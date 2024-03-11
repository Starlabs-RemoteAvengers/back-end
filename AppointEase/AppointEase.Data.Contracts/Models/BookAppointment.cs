using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppointEase.Data.Contracts.Models
{
    public class BookAppointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BookAppointmentId { get; set; } = Guid.NewGuid().ToString();
        public string AppointmentSlotId { get; set; }
        public string PatientId { get; set; }
        public string MeetingReason { get; set; }
        public string MeetingRequestDescription { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? ResponseDateTime { get; set; } = DateTime.Now;


        public virtual AppointmentSlot AppointmentSlot { get; set; }
        public virtual Patient Patient { get; set; }

    }
}