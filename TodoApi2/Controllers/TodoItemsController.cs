using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi2.Models;

namespace TodoApi2.Controllers
{
    // =====================================================
    // [Route] - 이 컨트롤러의 기본 URL 경로 설정
    // "api/[controller]" → "api/TodoItems" 가 됨
    // [controller]는 클래스명에서 "Controller"를 뺀 이름으로 자동 치환
    // =====================================================
    [Route("api/[controller]")]

    // =====================================================
    // [ApiController] - Web API 전용 컨트롤러임을 표시
    // 자동으로 해주는 것들:
    // - 모델 유효성 검사 자동 처리
    // - [FromBody] 자동 추론
    // - 400 Bad Request 자동 반환
    // =====================================================
    [ApiController]

    // =====================================================
    // ControllerBase 상속
    // - MVC의 Controller와 다름 (View 기능 없음)
    // - API 전용: Ok(), NotFound(), BadRequest() 등 제공
    // =====================================================
    public class TodoItemsController : ControllerBase
    {
        // =====================================================
        // DI(의존성 주입)로 받은 DB Context
        // readonly: 생성자에서만 할당 가능, 이후 변경 불가
        // Java로 치면: private final TodoRepository todoRepository;
        // =====================================================
        private readonly TodoContext _context;

        // =====================================================
        // 생성자 - DI 컨테이너가 TodoContext를 자동으로 넣어줌
        // Program.cs의 AddDbContext()에서 등록한 게 여기로 옴
        // 
        // spring으로 치면:
        // @Autowired
        // public TodoController(TodoRepository repo) { ... }
        // =====================================================
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // =====================================================
        // [HttpGet] - GET 요청 처리
        // URL: GET /api/TodoItems
        // 
        // 반환 타입 설명:
        // - Task<> : 비동기 처리 (Java의 CompletableFuture)
        // - ActionResult<> : HTTP 응답 + 데이터 (200 OK, 404 등)
        // - IEnumerable<TodoItem> : TodoItem 리스트
        // =====================================================

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            // ToListAsync(): SELECT * FROM TodoItems 실행
            return await _context.TodoItems.ToListAsync();
        }

        // =====================================================
        // [HttpGet("{id}")] - GET /api/TodoItems/5
        // {id}는 URL 경로 변수 → 메서드 파라미터 id로 바인딩
        // =====================================================

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            // FindAsync(): SELECT * FROM TodoItems WHERE Id = @id
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                // NotFound(): 404 응답 반환
                return NotFound();
            }

            // 200 OK + todoItem JSON 반환
            return todoItem;
        }

        // =====================================================
        // [HttpPut("{id}")] - PUT /api/TodoItems/5
        // 전체 수정 (PATCH는 부분 수정)
        // =====================================================

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            // URL의 id와 body의 id가 다르면 잘못된 요청
            if (id != todoItem.Id)
            {
                return BadRequest(); // 400 응답
            }

            // =====================================================
            // Entry().State = Modified
            // "이 엔티티가 수정됐으니 UPDATE 해줘"라고 EF에게 알려줌
            // 
            // Java/JPA 비교:
            // entityManager.merge(todoItem);
            // =====================================================
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                // 실제 UPDATE 쿼리 실행
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // 동시성 문제: 다른 사람이 먼저 수정/삭제했을 때
                if (!TodoItemExists(id))
                {
                    return NotFound(); // 이미 삭제됨
                }
                else
                {
                    throw; // 다른 에러면 그대로 던짐
                }
            }

            // NoContent(): 204 응답 (성공했지만 반환할 데이터 없음)
            return NoContent();
        }

        // =====================================================
        // [HttpPost] - POST /api/TodoItems
        // 새 항목 생성
        // 
        // Java 비교:
        // @PostMapping
        // public ResponseEntity<TodoItem> create(@RequestBody TodoItem item)
        // =====================================================

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            // Add(): INSERT 준비 (아직 DB에 안 들어감)
            _context.TodoItems.Add(todoItem);

            // SaveChangesAsync(): 실제 INSERT 실행, Id 자동 생성됨
            await _context.SaveChangesAsync();

            // =====================================================
            // CreatedAtAction(): 201 Created 응답
            // - "GetTodoItem" 액션을 참조해서 Location 헤더 생성
            // - Location: /api/TodoItems/1 (새로 만든 리소스 URL)
            // - Body에 생성된 todoItem 포함
            // 
            // Java 비교:
            // return ResponseEntity
            //     .created(URI.create("/api/todoitems/" + item.getId()))
            //     .body(item);
            // =====================================================
            //            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem); // nameof 연산자로 문자열 하드코딩 피하기

        }

        // =====================================================
        // [HttpDelete("{id}")] - DELETE /api/TodoItems/5
        // 
        // Java 비교:
        // @DeleteMapping("/{id}")
        // public ResponseEntity<?> delete(@PathVariable Long id)
        // =====================================================

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            // Remove(): DELETE 준비
            _context.TodoItems.Remove(todoItem);

            // 실제 DELETE 실행
            await _context.SaveChangesAsync();

            return NoContent(); // 204 응답
        }

        // =====================================================
        // 헬퍼 메서드: 해당 ID가 존재하는지 확인
        // Any(): 하나라도 있으면 true
        // SQL: SELECT COUNT(1) FROM TodoItems WHERE Id = @id
        // =====================================================
        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
