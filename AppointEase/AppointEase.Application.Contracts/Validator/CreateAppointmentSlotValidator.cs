using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;

namespace AppointEase.Application.Contracts.Validator
{
    public class CreateAppointmentSlotValidator : AbstractValidator<AppointmentSlotRequest>
    {
        public CreateAppointmentSlotValidator()
        {
            RuleFor(appointmentSlot => appointmentSlot.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.");

            RuleFor(appointmentSlot => appointmentSlot.ClinicId)
                .NotEmpty().WithMessage("ClinicId is required.");

            //RuleFor(appointmentSlot => appointmentSlot.AppointmentDateTime)
            //    .NotEmpty().WithMessage("AppointmentDateTime is required.")
            //    .GreaterThanOrEqualTo(DateTime.Now).WithMessage("AppointmentDateTime must be in the future.");

            //RuleFor(appointmentSlot => appointmentSlot.DurationInMinutes)
            //    .GreaterThanOrEqualTo(0).When(appointmentSlot => appointmentSlot.DurationInMinutes.HasValue)
            //    .WithMessage("DurationInMinutes must be greater than or equal to 0 if provided.");
        }
    }
}
