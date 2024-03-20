using AppointEase.Http.Contracts.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Validations
{
    public class RefundRequestValidator : AbstractValidator<RefundRequest>
    {
        public RefundRequestValidator()
        {
            RuleFor(request => request.PaymentIntentId).NotEmpty().WithMessage("PaymentIntentId is required.");
        }
    }
}
