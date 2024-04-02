using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Services
{
    public class StripeService : IStripeApi
    {
        private readonly IStripeApi _stripeApi;
        private readonly IConfiguration _configuration;
        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
            var secretKey = _configuration.GetSection("Stripe:SecretKey").Value;
            Console.WriteLine($"Secret Key: {secretKey}"); // Add this line for debugging
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
