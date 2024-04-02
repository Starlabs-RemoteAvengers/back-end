
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Data.Contracts.Models
{
    public class ChatMessages
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MessageId { get; set; } = Guid.NewGuid().ToString();
        public string SenderId { get; set; } 
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
