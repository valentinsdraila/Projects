using Braintree;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Services;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("api/braintree")]
    public class BraintreeController : ControllerBase
    {
        private readonly BraintreeGateway _gateway;

        public BraintreeController(BraintreeGatewayFactory factory)
        {
            _gateway = factory.Create();
        }

        [HttpGet("client-token")]
        public IActionResult GetClientToken()
        {
            var clientToken = _gateway.ClientToken.Generate();
            return Ok(new { clientToken });
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] PaymentRequest request)
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
                // Publish PaymentSucceeded event via RabbitMQ
                return Ok(result.Target);
            }

            return BadRequest(result.Message);
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Nonce { get; set; }
    }

}
