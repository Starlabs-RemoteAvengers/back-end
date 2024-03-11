using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string ClinicId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comments { get; set; }
        public string Location { get; set; }

        public DateTime RequestDateTime { get; set; } // Data dhe ora e kërkesës për takim
        public DateTime AppointmentDate { get; set; }

        public enum PaymentStatus
        {
            Pending,
            Paid,
            Unpaid
        }
        public PaymentStatus paymentStatu { get; set; }

        public string Status { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }

}
