using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointEase.Data.Contracts.Identity;

namespace AppointEase.Data.Contracts.Models
{
    public class ConnectionRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(FromId))]
        public ApplicationUser FromUser { get; set; }
        public string FromId { get; set; }

        [ForeignKey(nameof(ToId))]
        public ApplicationUser ToUser { get; set; }
        public string ToId { get; set; }

        public DateTime dateTimestamp { get; set; }
    }
}
