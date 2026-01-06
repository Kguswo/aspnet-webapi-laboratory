
using Dapper;
using DapperMvcDemo.Data;
using DapperMvcDemo.Models;

namespace DapperMvcDemo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperDbContext _context;

        public ProductRepository(DapperDbContext context)
        {
            _context = context;
        }

        /*
         * public async Task<T> 메서드명(파라미터)
         * 1. SQL작성
         * 2. 연결 생성 (try-with-resources), 자동종료
         * 3. 실행(await 비동기) - connection.~~
         *    복수SELECT - QueryAsync<T>               / return IEnumerable<T>
         *    단일SELECT - QueryFirstOrDefaultAsync<T> / return T or null
         *    INSERT - ExecuteAsync                    / return 저장한객체, void, id
         *    UPDATE - ExecuteAsync                    / return 수정한객체, bool, void
         *    DELETE - ExecuteAsync                    / return void, bool
         * 4. 결과 리턴
         */

        // 전체 조회
        public async Task<IEnumerable<ProductModel>> GetAll() // async로 비동기 메서드 선언, Task로 비동기 결과, IEnumerable<T>는 컬렉션 인터페이스
        {
            var sql = "SELECT * FROM Products";
            using var connection = _context.CreateConnection();
            // 블록 끝나면 자동으로 connection.Dispose() 호출 (연결 종료) (try-with-resources같은거) (ex. try (Connection conn = dataSource.getConnection()))
            return await connection.QueryAsync<ProductModel>(sql); // await로 비동기 결과 기다리기
        }

        // 단일 조회
        public async Task<ProductModel> GetById(Guid id)
        {
            var sql = "SELECT * FROM Products WHERE ProductId = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ProductModel>(sql, new { id });
        }

        // 생성
        public async Task<ProductModel> Create(ProductModel model)
        {
            model.ProductId = Guid.NewGuid();
            model.CreatedOn = DateTime.Now;

            var sql = @"INSERT INTO Products
                        (ProductId, ProductName, Price, ProductDescription, CreatedOn)
                        VALUES
                        (@ProductId, @ProductName, @Price, @ProductDescription, @CreatedOn)";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, model);
            return model;
        }

        // 수정
        public async Task<ProductModel> Update(ProductModel model)
        {
            model.UpdatedOn = DateTime.Now;

            var sql = @"UPDATE Products SET
                        ProductName = @ProductName,
                        Price = @Price,
                        ProductDescription = @ProductDescription,
                        UpdateOn = @UpdateOn
                        WHERE ProductId = @ProductId";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, model);
            return model;
        }

        // 삭제
        public async Task Delete(Guid id)
        {
            var sql = "DELETE FROM Products WHERE ProductId = @id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
