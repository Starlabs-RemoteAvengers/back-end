using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using Microsoft.Extensions.Configuration;
using Refit;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Services
{
    public class StripeService : IStripeApi
    {
        private readonly PaymentIntentService _paymentIntentService;
        private readonly CustomerService _customerService;

        public StripeService(IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("Stripe:SecretKey").Value;
            StripeConfiguration.ApiKey = secretKey;
            _paymentIntentService = new PaymentIntentService();
            _customerService = new CustomerService();
        }

        public async Task<string> CreatePaymentIntent(PaymentIntentRequest paymentIntentRequest)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = paymentIntentRequest.Amount,
                Currency = paymentIntentRequest.Currency,
                PaymentMethodTypes = paymentIntentRequest.PaymentMethodTypes,
                Customer = paymentIntentRequest.PatientId,
                PaymentMethod = paymentIntentRequest.PaymentMethod,
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
                PaymentIntent = refundRequest.PaymentIntentId,
            };

            try
            {
                var refund = await refundService.CreateAsync(refundOptions);
                return refund.Id;
            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

        public async Task<string> RegisterPatient(RegisterPatientRequest patient)
        {
            var options = new CustomerCreateOptions
            {
                Email = patient.Email,
                Name = $"{patient.Name} {patient.Surname}",
            };

            try
            {
                var customer = await _customerService.CreateAsync(options);

                var stripeCustomerId = customer.Id;

                return stripeCustomerId;
            }
            catch (StripeException ex)
            {
                throw ex;
            }
        }

    }
}