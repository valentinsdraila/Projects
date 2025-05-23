using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public interface IProductService
    {
        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        Task AddProduct(AddProductDto product, IFormFile image);
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of all the products.</returns>
        Task<List<Product>> GetAllProducts();
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The product</returns>
        Task<Product?> GetProduct(Guid productId);
        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task DeleteProduct(Product product);
        /// <summary>
        /// Deletes the product by identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        Task DeleteProductById(Guid productId);
        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task UpdateProduct(Product product);
        /// <summary>
        /// Searches the products.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        Task<List<Product>> SearchProducts(string query);
    }
}
