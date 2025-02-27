using ProductService.DataLayer;
using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductsService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProduct(Product product)
        {
            await this._productRepository.Add(product);
        }

        public async Task DeleteProduct(Product product)
        {
            await this._productRepository.Delete(product);
        }

        public async Task DeleteProductById(Guid productId)
        {
            await this._productRepository.DeleteById(productId);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await this._productRepository.GetAll();
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            return await this._productRepository.GetById(productId);
        }

        public async Task UpdateProduct(Product product)
        {
            await this._productRepository.Update(product);
        }
    }
}
