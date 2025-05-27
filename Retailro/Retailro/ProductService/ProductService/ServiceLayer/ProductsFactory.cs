using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public class ProductsFactory
    {
        /// <summary>
        /// Creates the product dto.
        /// </summary>
        /// <param name="cartItem">The cart item.</param>
        /// <returns>The product dto.</returns>
        public static CartItemDto CreateProductDto(CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.ProductId,
                Name = cartItem.Product.Name,
                Description = cartItem.Product.Description,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Product.UnitPrice * cartItem.Quantity,
                Image = cartItem.Product.Image
            };
        }
        /// <summary>
        /// Creates the cart item.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>The cart item.</returns>
        public static CartItem CreateCartItem(ProductDto product)
        {
            return new CartItem
            {
                ProductId = product.Id,
                Quantity = 1
            };
        }
    }
}
