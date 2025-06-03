using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Model;
using ProductService.Model.Dtos;
using ProductService.ServiceLayer;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProductService.Controllers
{
    /// <summary>
    /// Controller used for handling products functionality.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public ProductController(IProductService productService, RabbitMQPublisher rabbitMQPublisher)
        {
            this._productService = productService;
            this._rabbitMQPublisher = rabbitMQPublisher;
        }
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to fetch the products!", error = ex.Message });
            }
        }
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }
                Guid userIdGuid = Guid.Parse(userId);

                var product = await _productService.GetProduct(id);
                if (product == null)
                {
                    return NotFound();
                }
                await _rabbitMQPublisher.SendUserInteraction(new UserInteractionMessage { UserId = userIdGuid, ProductId = id, Action = InteractionType.View });
                return Ok(product);
            }
            catch (Exception ex) {
                return BadRequest(new { message = "Error while trying to fetch the product!", error = ex.Message });
            }

        }
        /// <summary>
        /// Adds the product.
        /// Available only for the administrators of the system.
        /// </summary>
        /// <param name="dto">The product data transfer object.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto dto, IFormFile image)
        {
            try
            {
                await _productService.AddProduct(dto, image);
                return Ok(new { message = "The product has been added!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to add the product!", error = ex.Message });
            }
        }
        /// <summary>
        /// Deletes the product by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            await this._productService.DeleteProductById(id);
            return NoContent();
        }
        /// <summary>
        /// Searches the products.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <param name="category">The category.</param>
        /// <param name="brand">The brand.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            string query,
            decimal? minPrice,
            decimal? maxPrice,
            string category = "",
            string brand = "",
            string sort = "relevance")
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return BadRequest(new { message = "The search string is empty!" });
                var products = await _productService.SearchProducts(query, category, brand, minPrice, maxPrice, sort);

                return Ok(new { products });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while fetching the products!", error = ex.Message });
            }
        }
        /// <summary>
        /// Gets the brands.
        /// </summary>
        /// <returns></returns>
        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            try
            {
                var brands = await _productService.GetBrands();
                return Ok(new { brands });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _productService.GetCategories();
                return Ok(new { categories });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("newest")]
        public async Task<IActionResult> GetNewestProducts()
        {
            try
            {
                var newest = await _productService.GetNewest();
                return Ok(new { newest });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }
        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedProducts([FromQuery(Name = "ids")] List<Guid> productIds)
        {
            try
            {
                var recommended = await _productService.GetRecommended(productIds);
                return Ok(new { recommended });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
