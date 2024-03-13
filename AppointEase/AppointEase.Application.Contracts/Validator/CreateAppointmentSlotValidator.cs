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

            RuleFor(appointmentSlot => appointmentSlot.StartTime)
                .NotEmpty().WithMessage("StartTime is required.");

            RuleFor(appointmentSlot => appointmentSlot.EndTime)
                .NotEmpty().WithMessage("EndTime is required.")
                .GreaterThan(appointmentSlot => appointmentSlot.StartTime.Add(TimeSpan.FromMinutes(29)))
                .WithMessage("EndTime must be greater than StartTime by at least 30 minutes.");

            RuleFor(appointmentSlot => appointmentSlot.IsBooked)
                .NotNull().WithMessage("IsBooked is required.");

            RuleFor(appointmentSlot => appointmentSlot.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(appointmentSlot => appointmentSlot.PatientId)
                .NotEmpty().When(appointmentSlot => appointmentSlot.IsBooked)
                .WithMessage("PatientId is required when appointment is booked.");
        }
    }
}
