using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.ServiceLayer;
using System.Security.Claims;

namespace OrderService.Controllers
{
    /// <summary>
    /// Controller used for handling orders.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Places the order.
        /// </summary>
        /// <param name="productInfos">The product infos.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(List<ProductInfo> productInfos)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userGuid = Guid.Parse(userId);
                var orderResponse = await _orderService.AddOrder(productInfos, userGuid);
                return Ok(new { message = "The order has been placed!", order = orderResponse });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = "An error occured while placing the order.", error = ex.Message});
            }
        }
        /// <summary>
        /// Gets the user orders.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userGuid = Guid.Parse(userId);
                var orders = await _orderService.GetAllOrdersForUser(userGuid);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occured while fetching the user orders the order.", error = ex.Message });
            }
        }
        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(Guid orderId)
        {
            try
            {
                return Ok(await _orderService.GetOrderById(orderId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occured while fetching the order.", error = ex.Message });
            }
        }
    }
}
