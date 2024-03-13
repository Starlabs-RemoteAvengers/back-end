using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class AppointmentSlotRequest
    {
        public AppointmentSlotRequest()
        {
            AppointmentSlotId = Guid.NewGuid().ToString();
        }
        public string AppointmentSlotId { get; private set; }
        public string DoctorId { get; set; }
        public string ClinicId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
        public DateOnly Date { get; set; }
        public string? PatientId { get; set; }
    }
}
