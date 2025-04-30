using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
using OrderService.ServiceLayer;
using System.Security.Claims;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

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
                await _orderService.AddOrder(productInfos, userGuid);
                return Ok(new { message = "The order has been placed!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = "An error occured while placing the order.", error = ex.Message});
            }
        }
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
