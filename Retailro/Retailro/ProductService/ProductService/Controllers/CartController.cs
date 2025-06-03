using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;
using ProductService.Model.Dtos;
using ProductService.ServiceLayer;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductService.Controllers
{
    /// <summary>
    /// Controller used in handling cart functionality.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public CartController(ICartService cartService, IProductService productService, RabbitMQPublisher rabbitMQPublisher)
        {
            this._cartService = cartService;
            this._productService = productService;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        /// <summary>
        /// Gets all products in cart.
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetAllProductsInCart()
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userIdGuid = Guid.Parse(userId);
                var products = await _cartService.GetProductsInCart(userIdGuid);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to get all products in the cart!", error = ex.Message });
            }
        }
        /// <summary>
        /// Adds the product to cart.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        [HttpPost("products/{productId}")]
        public async Task<IActionResult> AddProductToCart(Guid productId)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userIdGuid = Guid.Parse(userId);
                await _cartService.AddItemToCart(productId, userIdGuid, _productService);
                await _rabbitMQPublisher.SendUserInteraction(new UserInteractionMessage { UserId = userIdGuid, ProductId = productId, Action = InteractionType.AddToCart });
                return Ok(new { status = "success", message = "The product has been added to the cart." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to add the product to the cart!", error = ex.Message });
            }
        }
        /// <summary>
        /// Removes the product from cart.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        [HttpDelete("products/{productId}")]
        public async Task<ActionResult> RemoveProductFromCart(Guid productId)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userIdGuid = Guid.Parse(userId);
                await this._cartService.RemoveProductFromCart(productId, userIdGuid);
                return Ok(new { message = "The item was removed from the cart." });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// Gets the cart.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(Guid id)
        {
            try
            {
                var cart = await _cartService.GetCart(id);
                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to fetch the cart!", error = ex.Message });
            }
        }
        /// <summary>
        /// Adds the cart.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddCart(Cart cart)
        {
            try
            {
                await this._cartService.AddCart(cart);
                return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
            }
            catch (ValidationException)
            {
                return BadRequest(new { message = "The product data is invalid." });
            }
        }
        /// <summary>
        /// Deletes the cart by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartById(Guid id)
        {
            try
            {
                await this._cartService.DeleteCartById(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to delete the cart!", error = ex.Message });
            }
        }
        /// <summary>
        /// Clears the cart.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userIdGuid = Guid.Parse(userId);
                await this._cartService.ClearCart(userIdGuid);
                return Ok(new { message = "The cart was cleared." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
