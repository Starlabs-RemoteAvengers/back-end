using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using Stripe;
using System.Collections.Generic;

namespace AppointEase.Http.Services
{
    public class StripeService : IStripeApi
    {
        private readonly PaymentIntentService _paymentIntentService;

        public StripeService(IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("Stripe:SecretKey").Value;
            StripeConfiguration.ApiKey = secretKey;
            _paymentIntentService = new PaymentIntentService();
        }

        public async Task<string> CreatePaymentIntent(PaymentIntentRequest paymentIntentRequest)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentIntentRequest.Amount,
                Currency = paymentIntentRequest.Currency,
                PaymentMethodTypes = paymentIntentRequest.PaymentMethodTypes,
                Customer = paymentIntentRequest.UserId,
                PaymentMethod = paymentIntentRequest.PaymentMethod 
            };

            var paymentIntent = await _paymentIntentService.CreateAsync(options);
            var clientSecret = paymentIntent.ClientSecret;

            return clientSecret;
        }

        public async Task<string> RefundPayment(RefundRequest refundRequest)
        {
         

            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = refundRequest.PaymentIntentId, // Retrieve Payment Intent ID from the dictionary using user ID
            };

            try
            {
                var refund = await refundService.CreateAsync(refundOptions);
                return refund.Id;
            }
            catch (StripeException ex)
            {
                // Handle Stripe exceptions here
                throw ex;
            }
        }
    }
}
