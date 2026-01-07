using Microsoft.EntityFrameworkCore;

namespace TodoApi2.Models
{
    /*
     * DbContext: EF Core에서 DB와 통신하는 핵심 클래스
     * - DB 연결관리
     * - 쿼리 실행
     * - 변경 사항 추적
     * - 트랜잭션 관리
     */
    public class TodoContext : DbContext
    {
        // 생성자: DI(의존성 주입)를 통해 DB 설정을 받아옴
        // DbContextOptions: 연결 문자열, DB 종류(SQL Server, SQLite 등) 정보 포함
        // Program.cs에서 AddDbContext()로 설정한 옵션이 여기로 들어옴
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options) // 부모 클래스(DbContext)에 옵션 전달
        {
        }

        /* 
         * DbSet<T>: DB의 테이블을 C# 컬렉션처럼 다룰 수 있게 해줌
         * TodoItems라는 이름의 DbSet = DB의 TodoItems 테이블과 매핑
         * 
         * 사용 예시:
         * _context.TodoItems.Add(item);      // INSERT
         * _context.TodoItems.Find(id);       // SELECT by PK
         * _context.TodoItems.ToList();       // SELECT *
         * _context.TodoItems.Remove(item);   // DELETE
         *
         */
        public DbSet<TodoItem> TodoItems { get; set; } = null!; // null!은 이것이 null이 아니다라는걸 컴파일러한테 알려준다.
                                                                // EF Core가 런타임에 값을 채워줄 것이기 때문에 null 가능성은 없다.
    }
}
