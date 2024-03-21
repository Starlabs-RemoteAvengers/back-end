using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AppointEase.Application.Contracts.Validator
{
    public class CreateDoctorValidator : AbstractValidator<DoctorRequest>
    {
        public CreateDoctorValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname is required.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.");
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email is required.")
           .Matches(@"^[^\s@]+@[^\s@]+\.[^\s@]+(\.[^\s@]+)*$").WithMessage("Invalid email format.");
            RuleFor(x => x.Password).Must(password => password == null || (password.Length >= 8
                                           && Regex.IsMatch(password, "[A-Z]")
                                           && Regex.IsMatch(password, "[a-z]")
                                           && Regex.IsMatch(password, "[0-9]")
                                           && Regex.IsMatch(password, "[^a-zA-Z0-9]"))).When(x => x.Password != null).WithMessage("Password must have 8 characters or more and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required.");
            //RuleFor(x => x.DoctorName).NotEmpty().WithMessage("DoctorName is required.");
            RuleFor(x => x.Specialisation).NotEmpty().WithMessage("Specialisation is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.DateOfBirth)
              .NotEmpty().WithMessage("DateOfBirth is required.")
              .Must(date => date != default(DateOnly)).WithMessage("Invalid DateOfBirth.")
              .Must(dateOfBirth => IsAtLeast21YearsOld(dateOfBirth)).WithMessage("You must be at least 21 years old.");

        }
        private bool IsAtLeast21YearsOld(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;

            // Subtract one if the birthday for this year hasn't occurred yet
            if (dateOfBirth.DayOfYear > today.DayOfYear)
                age--;

            return age >= 21;
        }
    }
}
