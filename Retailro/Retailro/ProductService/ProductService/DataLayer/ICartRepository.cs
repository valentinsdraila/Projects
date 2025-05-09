using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface ICartRepository
    {
        /// <summary>
        /// Gets the cart by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Cart?> GetById(Guid id);
        /// <summary>
        /// Gets the cart by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<Cart?> GetByUserId(Guid userId);
        /// <summary>
        /// Gets all the carts.
        /// </summary>
        /// <returns></returns>
        Task<List<Cart>> GetAll();
        /// <summary>
        /// Adds the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task Add(Cart cart);
        /// <summary>
        /// Updates the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task Update(Cart cart);
        /// <summary>
        /// Deletes the cart by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task DeleteById(Guid id);
        /// <summary>
        /// Deletes the cart by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task DeleteByUserId(Guid userId);
        /// <summary>
        /// Deletes the specified cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task Delete(Cart cart);
        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
