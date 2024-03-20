using AppointEase.Http.Contracts.Requests;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Contracts.Interfaces
{
    public interface IStripeService
    {
        [Post("/api/stripe/charge")]
        Task<string> Charge([Body] PaymentRequest paymentRequest);

        [Post("/api/stripe/refund")]
        Task<string> Refund([Body] RefundRequest refundRequest);
    }
}
