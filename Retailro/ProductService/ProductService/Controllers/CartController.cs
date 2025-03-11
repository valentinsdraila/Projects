using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Model;
using ProductService.ServiceLayer;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public CartController(ICartService cartService, IProductService productService)
        {
            this._cartService = cartService;
            this._productService = productService;
        }
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsInCart()
        {
            var userId = Request.Headers["x-user-id"].FirstOrDefault();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in request.");
            }

            Guid userIdGuid = Guid.Parse(userId);
            var products = await _cartService.GetProductsInCart(userIdGuid);
            return Ok(products);
        }
        [HttpPost("products/{productId}")]
        public async Task<ActionResult> AddProductToCart(Guid productId)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in request.");
                }

                Guid userIdGuid = Guid.Parse(userId);
                await _cartService.AddItemToCart(productId, userIdGuid, _productService);
                return Ok("The product has been added to the cart.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("products/{productId}")]
        public async Task<ActionResult> RemoveProductFromCart(Guid productId)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in request.");
                }

                Guid userIdGuid = Guid.Parse(userId);
                await this._cartService.RemoveProductFromCart(productId, userIdGuid);
                return Ok("The item was removed from the cart.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(Guid id)
        {
            var cart = await _cartService.GetCart(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
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
                return BadRequest("The product data is invalid.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartById(Guid id)
        {
            await this._cartService.DeleteCartById(id);
            return NoContent();
        }
    }
}
