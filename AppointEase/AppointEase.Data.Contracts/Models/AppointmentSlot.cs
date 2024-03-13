using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Models
{
    public class AppointmentSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AppointmentSlotId { get; set; } = Guid.NewGuid().ToString();
        public string DoctorId { get; set; }
        public string ClinicId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
        public DateOnly Date { get; set; }
        public string? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}
