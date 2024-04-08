using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Contracts.Requests
{
    public class PaymentIntentRequest
    {
        [Key]
        public int Amount { get; set; }
        public string Currency { get; set; }
        public List<string>? PaymentMethodTypes { get; set; }
        public string PaymentMethod { get; set; }
        //public string UserId { get; set; }
        public string PatientId { get; set; }

    }
}