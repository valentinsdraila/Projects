using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public interface IProductService
    {
        Task AddProduct(Product product);
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProduct(Guid productId);
        Task DeleteProduct(Product product);
        Task DeleteProductById(Guid productId);
        Task UpdateProduct(Product product);
    }
}
