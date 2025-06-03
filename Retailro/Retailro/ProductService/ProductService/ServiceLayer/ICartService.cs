using ProductService.DataLayer;
using ProductService.Model;
using ProductService.Model.Dtos;

namespace ProductService.ServiceLayer
{
    public interface ICartService
    {
        /// <summary>
        /// Adds the cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task AddCart(Cart cart);
        /// <summary>
        /// Gets all carts.
        /// </summary>
        /// <returns>A list containing all the carts</returns>
        Task<List<Cart>> GetAllCarts();
        /// <summary>
        /// Gets the cart.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <returns>The cart</returns>
        Task<Cart?> GetCart(Guid cartId);
        /// <summary>
        /// Deletes the cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task DeleteCart(Cart cart);
        /// <summary>
        /// Deletes the cart by identifier.
        /// </summary>
        /// <param name="cartId">The cart identifier.</param>
        /// <returns></returns>
        Task DeleteCartById(Guid cartId);
        /// <summary>
        /// Updates the cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        Task UpdateCart(Cart cart);
        /// <summary>
        /// Gets the cart by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The cart</returns>
        Task<Cart?> GetCartByUserId(Guid userId);
        /// <summary>
        /// Deletes the cart by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task DeleteCartByUserId(Guid userId);
        /// <summary>
        /// Adds the item to cart.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="productService">The product service.</param>
        /// <returns></returns>
        Task AddItemToCart(Guid productId, Guid userId, IProductService productService);
        /// <summary>
        /// Gets the products in cart.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<CartItemDto>> GetProductsInCart(Guid userId);
        /// <summary>
        /// Removes the product from cart.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A list of ProductDtos</returns>
        Task RemoveProductFromCart(Guid productId, Guid userId);
        /// <summary>
        /// Clears the cart.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task ClearCart(Guid userId);
    }
}
