using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class NotificationRequest
    {
        public NotificationRequest()
        {
            IdNotification = Guid.NewGuid().ToString();
        }
        public string? IdNotification { get; set; } 
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? FromId { get; set; }
        public string? ToId { get; set; }
        public string? Type { get; set; }
        public string? IdType { get; set; }
        public string? MessageType { get; set; }
        public bool? IsRead { get; set; } = false;
        public DateTime? dateTimestamp { get; set; }
    }
}
