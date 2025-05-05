using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface IProductRepository
    {
        Task<Product?> GetById(Guid id);
        Task<List<Product>> GetAll();
        Task Add(Product product);
        Task Update(Product product);
        Task DeleteById(Guid id);
        Task Delete(Product product);
        Task<bool> ReduceStock(Guid productId, int quantity);
        Task<bool> SaveChangesAsync();
    }
}
