using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class BookAppointmentRequest
    {
        public BookAppointmentRequest()
        {
            BookAppointmentId = Guid.NewGuid().ToString();
        }
        public string BookAppointmentId { get; private set; }
        public string AppointmentSlotId { get; set; }
        public string PatientId { get; set; }
        public string MeetingReason { get; set; }
        public string MeetingRequestDescription { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? ResponseDateTime { get; set; } = DateTime.Now;
    }
}