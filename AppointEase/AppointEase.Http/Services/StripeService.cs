using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Threading.Tasks;

namespace AppointEase.Http.Services
{
    public class StripeService : IStripeApi
    {
        private readonly IStripeApi _stripeApi;

        public StripeService(IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("Stripe:SecretKey").Value;
            _stripeApi = RestService.For<IStripeApi>("https://api.stripe.com", new RefitSettings
            {
                AuthorizationHeaderValueGetter = (request, cancellationToken) => Task.FromResult($"Bearer {secretKey}")
            });

        }

        public async Task<string> Charge(PaymentRequest paymentRequest)
        {
            return await _stripeApi.Charge(paymentRequest);
        }

        public async Task<string> Refund(RefundRequest refundRequest)
        {
            return await _stripeApi.Refund(refundRequest);
        }
    }
}
