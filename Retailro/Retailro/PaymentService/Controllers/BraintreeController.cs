using Braintree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PaymentService.Model;
using PaymentService.Model.Messages;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    /// <summary>
    /// Controller used for handling payments within the application.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/braintree")]
    public class BraintreeController : ControllerBase
    {
        private readonly BraintreeGateway _gateway;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public BraintreeController(BraintreeGatewayFactory factory, RabbitMQPublisher rabbitMQPublisher)
        {
            _gateway = factory.Create();
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        /// <summary>
        /// Gets the client token.
        /// </summary>
        /// <returns></returns>
        [HttpGet("client-token")]
        public IActionResult GetClientToken()
        {
            var clientToken = _gateway.ClientToken.Generate();
            return Ok(new { clientToken });
        }

        /// <summary>
        /// Checkout for the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="redis">The redis.</param>
        /// <returns></returns>
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] PaymentRequest request, [FromServices] RedisService redis)
        {
            var status = await redis.GetOrderStatusAsync(request.OrderId);

            if (status is null)
                return BadRequest("Order not found");

            switch (status)
            {
                case OrderStatus.Valid:
                    {
                        var result = await _gateway.Transaction.SaleAsync(new TransactionRequest
                        {
                            Amount = request.Amount,
                            PaymentMethodNonce = request.Nonce,
                            Options = new TransactionOptionsRequest
                            {
                                SubmitForSettlement = true
                            }
                        });

                        if (result.IsSuccess())
                        {
                            await redis.SetOrderStatusAsync(request.OrderId, OrderStatus.Paid, request.Amount);
                            var message = new PaymentStatusUpdateMessage { Id = request.OrderId, Status = OrderStatus.Paid };
                            await _rabbitMQPublisher.SendPaymentStatus(message);
                            return Ok(result.Target);
                        }

                        return BadRequest(result.Message);
                    }
                case OrderStatus.Cancelled:
                    {
                        return BadRequest(new { message = "The stock could not be confirmed so the order was cancelled" });
                    }
                case OrderStatus.Processing:
                    {
                        return BadRequest(new { message = "The stock has not been confirmed yet, please try again later!" });
                    }
                default:
                    {
                        return BadRequest(new { message = "The order has been paid already!" });
                    }
            }
        }
        /// <summary>
        /// Gets the order status.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="redis">The redis service.</param>
        /// <returns></returns>
        [HttpGet("order-status/{orderId}")]
        public async Task<IActionResult> GetOrderStatus(Guid orderId, [FromServices] RedisService redis)
        {
            var status = await redis.GetOrderStatusAsync(orderId);
            if (status == null) return NotFound();

            return Ok(new { status });
        }

    }
    /// <summary>
    /// Represents a payment request as received from the client application
    /// </summary>
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Nonce { get; set; }
        public Guid OrderId { get; set; }
    }
    public class RedisSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }


}
