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
        /// Reduces the stock of a given product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <returns></returns>
        Task<bool> ReduceStock(Guid productId, int quantity, decimal price);
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
