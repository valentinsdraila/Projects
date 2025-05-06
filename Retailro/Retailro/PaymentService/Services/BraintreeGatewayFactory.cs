using Braintree;
using Microsoft.Extensions.Options;

namespace PaymentService.Services
{
    public class BraintreeSettings
    {
        public string? Environment { get; set; }
        public string? MerchantId { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
    }

    public class BraintreeGatewayFactory
    {
        private readonly BraintreeSettings _settings;

        public BraintreeGatewayFactory(IOptions<BraintreeSettings> settings)
        {
            _settings = settings.Value;
        }

        public BraintreeGateway Create()
        {
            return new BraintreeGateway(
                _settings.Environment == "sandbox" ? Braintree.Environment.SANDBOX : Braintree.Environment.PRODUCTION,
                _settings.MerchantId,
                _settings.PublicKey,
                _settings.PrivateKey
            );
        }
    }

}
