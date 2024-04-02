//using AppointEase.Http.Contracts.Requests;
//using FluentValidation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AppointEase.Http.Validations
//{
//    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
//    {
//        public PaymentRequestValidator()
//        {
//            RuleFor(request => request.Amount).GreaterThan(0).WithMessage("Amount should be greater than 0.");
//            RuleFor(request => request.UserId).NotEmpty().WithMessage("UserId is required.");
//            RuleFor(request => request.CardNumber).CreditCard().WithMessage("Invalid credit card number.");
//            RuleFor(request => request.ExpMonth).InclusiveBetween(1, 12).WithMessage("Invalid expiration month.");
//            RuleFor(request => request.ExpYear).InclusiveBetween(DateTime.Now.Year, DateTime.Now.Year + 10).WithMessage("Invalid expiration year.");
//            RuleFor(request => request.Cvc).NotEmpty().WithMessage("CVC is required.");
//        }
//    }
//}
