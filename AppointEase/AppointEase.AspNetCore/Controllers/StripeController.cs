using AppointEase.Http.Contracts.Interfaces;
using AppointEase.Http.Contracts.Requests;
using AppointEase.Http.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppointEase.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly IStripeApi _stripeService;

        public StripeController(IStripeApi stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest paymentIntentRequest)
        {
            var clientSecret = await _stripeService.CreatePaymentIntent(paymentIntentRequest);
            return Ok(clientSecret);
        }

        [HttpPost("refund")]
        public async Task<IActionResult> Refund([FromBody] RefundRequest refundRequest)
        {
            var refundId = await _stripeService.RefundPayment(refundRequest);
            return Ok(refundId);
        }

        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequest registerPatientRequest)
        {
            var stripeCustomerId = await _stripeService.RegisterPatient(registerPatientRequest);
            return Ok(stripeCustomerId);
        }

    }
}