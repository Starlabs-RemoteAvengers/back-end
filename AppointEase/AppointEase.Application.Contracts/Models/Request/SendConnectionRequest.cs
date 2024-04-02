using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class SendConnectionRequest
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public string FromId { get; set; }

        public string ToId { get; set; }

        public DateTime dateTimestamp { get; set; }
    }
}
