using AppointEase.Data.Contracts.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Models
{
    public class Notifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IdNotification { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string Type { get; set; }
        public string? IdType { get; set; }
        public string MessageType { get; set; }
        public bool IsRead { get; set; }
        public DateTime dateTimestamp { get; set; }
    }
}
