using DapperMvcDemo.Models;
using DapperMvcDemo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperMvcDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        // 생성자 주입 - DI
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: /Products
        // 전체 상품 조회
        public async Task<IActionResult> Index() // IActionResult란 Controller 메서드가 반환할 수 있는 모든 응답 타입의 인터페이스
        {
            var products = await _productRepository.GetAll();
            return View(products);
        }

        // GET: /Products/Details/{id}
        // 단일 상품 상세 조회
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: /Products/Create
        // 상품 등록 폼
        public IActionResult Create()
        {
            return View(); // 뷰 반환
        }

        // POST: /Products/Create
        // 상품 등록 처리
        [HttpPost] // HTTP POST 메서드 지정 (java의 @PostMapping)
        [ValidateAntiForgeryToken] // CSRF 방어
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.Create(model);
                return RedirectToAction(nameof(Index)); // 다른 액션으로 리다이렉트 (redirect:/path 느낌)
            }
            return View(model); // 모델과 함께 뷰 반환
        }

        // GET: /Products/Edit/{id}
        // 상품 수정 폼
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST
        // 상품 수정처리
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductModel model)
        {
            if (id != model.ProductId)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _productRepository.Update(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: /Products/Delete/{id}
        // 상품 삭제 처리
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);  // 삭제 확인 페이지
        }

        // POST: /Products/DeleteConfirmed/{id}
        // 상품 삭제 확정 처리
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _productRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
