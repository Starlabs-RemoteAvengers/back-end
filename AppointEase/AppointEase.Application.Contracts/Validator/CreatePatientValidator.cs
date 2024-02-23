using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;

namespace AppointEase.Application.Contracts.Validator
{
    public class CreatePatientValidator : AbstractValidator<PatientRequest>
    {
        public CreatePatientValidator()
        {
            RuleFor(person => person.PersonalNumber).NotEmpty().WithMessage("Personal Number is required.");
            RuleFor(person => person.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(person => person.Surname).NotEmpty().WithMessage("Surname is required.");
            //RuleFor(person => person.Gender).NotEmpty().WithMessage("Must select your Gander.");
            //RuleFor(person => person.Address).NotEmpty().WithMessage("Your Address is required.");
            //RuleFor(person => person.ContactNumber).NotEmpty().WithMessage("Please write your phone number");
            RuleFor(person => person.Email).EmailAddress().NotEmpty().WithMessage("Email address is required.");
            RuleFor(person => person.Password).MinimumLength(8).WithMessage("Password must have 8 charcters or more!");
            RuleFor(person => person.Role).NotEmpty().WithMessage("You need to specify Role!");
        }

    }
}
