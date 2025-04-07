using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public class ProductsFactory
    {
        public static ProductDto CreateProductDto(CartItem cartItem)
        {
            return new ProductDto
            {
                Id = cartItem.ProductId,
                Name = cartItem.Product.Name,
                Description = cartItem.Product.Description,
                Quantity = cartItem.Product.Quantity,
                TotalPrice = cartItem.Product.UnitPrice * cartItem.Quantity,
                Image = cartItem.Product.Image
            };
        }
        public static CartItem CreateCartItem(Product product)
        {
            return new CartItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1
            };
        }
    }
}
