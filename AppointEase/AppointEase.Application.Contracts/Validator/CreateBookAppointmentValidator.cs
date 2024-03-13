using AppointEase.Application.Contracts.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Validator
{
    public class CreateBookAppointmentValidator : AbstractValidator<BookAppointmentRequest>
    {
        public CreateBookAppointmentValidator()
        {
            RuleFor(x => x.AppointmentSlotId).NotEmpty().WithMessage("AppointmentSlotId is required.");
            RuleFor(x => x.PatientId).NotEmpty().WithMessage("PatientId is required.");
            RuleFor(x => x.MeetingReason).NotEmpty().WithMessage("MeetingReason is required.");
            RuleFor(x => x.MeetingRequestDescription).NotEmpty().WithMessage("MeetingRequestDescription is required.");
        }
    }
}
