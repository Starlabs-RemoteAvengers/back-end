using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Services
{
    public class StripeService : IStripeService
    {
        public StripeService()
        {
        }

        public async Task<string> Charge(PaymentRequest paymentRequest)
        {
            return await RestService.For<IStripeService>("https://api.stripe.com").Charge(paymentRequest);
        }

        public async Task<string> Refund(RefundRequest refundRequest)
        {
            return await RestService.For<IStripeService>("https://api.stripe.com").Refund(refundRequest);
        }
    }
}
