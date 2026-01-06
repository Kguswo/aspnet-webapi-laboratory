using DapperMvcDemo.Models;

namespace DapperMvcDemo.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAll(); // Task<T>란 비동기 작업의 결과를 나타내는 제네릭 인터페이스입니다. (Java의 CompletableFuture<T>) 
        Task<ProductModel> GetById(Guid id); // global unique identifier
        Task<ProductModel> Create(ProductModel model);
        Task<ProductModel> Update(ProductModel model);
        Task Delete(Guid id);
    }
}
