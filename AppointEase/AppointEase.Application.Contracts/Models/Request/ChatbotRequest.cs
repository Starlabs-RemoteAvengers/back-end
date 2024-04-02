using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class ChatbotRequest
    {
        public string? UserId { get; set; }
        public string? Problem { get; set; }
        public string? Options { get; set; }
        public string? FreeTextQuestion { get; set; }
    }
}
