using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Contracts.Requests
{
    public class RefundRequest
    {
        public string PaymentIntentId { get; set; }
    }
}
