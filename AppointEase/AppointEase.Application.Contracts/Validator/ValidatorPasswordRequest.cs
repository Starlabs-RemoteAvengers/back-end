using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Validator
{
    public class ValidatorPasswordRequest : AbstractValidator<PasswordRequest>
    {
        public ValidatorPasswordRequest()
        {
            RuleFor(x => x.NewPassword).Must(password => password == null || (password.Length >= 8
                                          && Regex.IsMatch(password, "[A-Z]")
                                          && Regex.IsMatch(password, "[a-z]")
                                          && Regex.IsMatch(password, "[0-9]")
                                          && Regex.IsMatch(password, "[^a-zA-Z0-9]"))).When(x => x.NewPassword != null).WithMessage("Password must have 8 characters or more and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }
    }
}
