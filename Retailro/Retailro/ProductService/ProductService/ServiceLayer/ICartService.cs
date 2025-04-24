using ProductService.DataLayer;
using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public interface ICartService
    {
        Task AddCart(Cart cart);
        Task<List<Cart>> GetAllCarts();
        Task<Cart?> GetCart(Guid cartId);
        Task DeleteCart(Cart cart);
        Task DeleteCartById(Guid cartId);
        Task UpdateCart(Cart cart);
        Task<Cart?> GetCartByUserId(Guid userId);
        Task DeleteCartByUserId(Guid userId);
        Task AddItemToCart(Guid productId, Guid userId, IProductService productService);
        Task<List<ProductDto>> GetProductsInCart(Guid userId);
        Task RemoveProductFromCart(Guid productId, Guid userId);
    }
}
