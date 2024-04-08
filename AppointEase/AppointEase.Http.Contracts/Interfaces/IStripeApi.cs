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
    public interface IStripeApi
    {
        [Post("/stripe/create-payment-intent")]
        Task<string> CreatePaymentIntent([Body] PaymentIntentRequest paymentIntentRequest);

        [Post("/stripe/refund")]
        Task<string> RefundPayment([Body] RefundRequest refundRequest);

        [Post("register-patient")]
        Task<string> RegisterPatient([Body] RegisterPatientRequest patient);
    }
}