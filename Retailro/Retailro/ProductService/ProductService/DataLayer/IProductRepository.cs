using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface IProductRepository
    {
        /// <summary>
        /// Gets the product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Product?> GetById(Guid id);
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAll();
        /// <summary>
        /// Adds the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task Add(Product product);
        /// <summary>
        /// Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task Update(Product product);
        /// <summary>
        /// Deletes the product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeleteById(Guid id);
        /// <summary>
        /// Deletes the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        Task Delete(Product product);
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();

        /// <summary>
        /// Searches the products.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IQueryable<Product> SearchProducts(string query, string category, string brand, decimal? minPrice, decimal? maxPrice);
        /// <summary>
        /// Gets the brands.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBrands();
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetCategories();
        /// <summary>
        /// Gets the newest.
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetNewest(int numberOfProducts);
        /// <summary>
        /// Gets the recommended products.
        /// </summary>
        /// <param name="productIds">The product ids.</param>
        /// <returns></returns>
        Task<List<Product>> GetRecommended(List<Guid> productIds);
    }
}
