using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace AppointEase.Data.Contracts.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AppointmentId { get; set; } = Guid.NewGuid().ToString();
        public string BookAppointmentId { get; set; }
        public string Status { get; set; }
        //public DateTime DateTime { get; set; }
        public virtual BookAppointment BookAppointment { get; set; }
    }

}
