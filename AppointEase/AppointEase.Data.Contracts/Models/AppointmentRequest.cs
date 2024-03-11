
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppointEase.Data.Contracts.Models
{
    public class AppointmentRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AppointmentRequestId { get; set; } = Guid.NewGuid().ToString();
        public string AppointmentSlotId { get; set; }
        public string PatientId { get; set; }
        public string MeetingReason { get; set; }
        public string MeetingRequestDescription { get; set; }
        public string ServiceCategory { get; set; }
        public bool IsAccepted { get; set; }
        public int Priority { get; set; }
        public DateTime? ResponseDateTime { get; set; } = DateTime.Now;
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
