using AppointEase.Application.Contracts.Models;
using FluentValidation;

namespace AppointEase.AspNetCore.Validator
{
    public class PersonDtoValidator : AbstractValidator<PersonDto>
    {
        public PersonDtoValidator()
        {
            RuleFor(person => person.Emri).NotEmpty().WithMessage("Name is required.");
            RuleFor(person => person.Mbiemri).NotEmpty().WithMessage("Surname is required.");
            RuleFor(person => person.Mosha).GreaterThan(0).WithMessage("Age must be greater than 0.");
        }
    }
}
