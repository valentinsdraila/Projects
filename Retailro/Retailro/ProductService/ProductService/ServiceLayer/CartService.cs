using ProductService.DataLayer;
using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public async Task AddCart(Cart cart)
        {
            await cartRepository.Add(cart);
        }

        public async Task DeleteCart(Cart cart)
        {
            await cartRepository.Delete(cart);
        }

        public async Task DeleteCartById(Guid cartId)
        {
            await cartRepository.DeleteById(cartId);
        }

        public async Task DeleteCartByUserId(Guid userId)
        {
            await cartRepository.DeleteByUserId(userId);
        }

        public async Task<List<Cart>> GetAllCarts()
        {
            return await cartRepository.GetAll();
        }

        public async Task<Cart?> GetCart(Guid cartId)
        {
            var cart = await cartRepository.GetById(cartId);
            if (cart == null)
                cart = new Cart();
            return cart;
        }

        public async Task<Cart?> GetCartByUserId(Guid userId)
        {
            var cart = await cartRepository.GetByUserId(userId);
            if (cart == null)
                cart = new Cart();
            return cart;
        }

        public async Task UpdateCart(Cart cart)
        {
            await cartRepository.Update(cart);
        }
        public async Task AddItemToCart(Guid productId, Guid userId, IProductService productService)
        {
            try
            {
                Cart cart = await cartRepository.GetByUserId(userId);
                if (cart == null)
                {
                    cart = new Cart { Id = Guid.NewGuid(), UserId = userId, Products = new List<CartItem>() };
                    await cartRepository.Add(cart);
                }

                if (cart.Products == null)
                {
                    cart.Products = new List<CartItem>();
                }

                ProductDto product = await productService.GetProduct(productId);
                if (product == null)
                {
                    Console.WriteLine("Product not found: " + productId);
                    return;
                }
                var cartItem = ProductsFactory.CreateCartItem(product);
                cart.Products.Add(cartItem);
                Console.WriteLine(cartItem.ProductId + " :CartItem.ProductId");
                Console.WriteLine(cart.Products[0].ProductId + " :Cart.Products[0].ProductId");
                await cartRepository.Update(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding product to cart: \n" + ex);
            }
        }

        public async Task<List<CartItemDto>> GetProductsInCart(Guid userId)
        {
            var cart = await cartRepository.GetByUserId(userId);
            if (cart.Products == null)
                cart.Products = new List<CartItem>();
            List<CartItemDto> productDtos = cart.Products.Select(p => ProductsFactory.CreateCartItemDto(p)).ToList();
            return productDtos;
        }

        public async Task RemoveProductFromCart(Guid productId, Guid userId)
        {
            var cart = await cartRepository.GetByUserId(userId);
            cart.Products.RemoveAll(p => p.ProductId == productId);
            await cartRepository.Update(cart);
        }

        public async Task ClearCart(Guid userId)
        {
            var cart = await cartRepository.GetByUserId(userId);
            cart.Products = new List<CartItem>();
            await cartRepository.Update(cart);
        }
    }
}
