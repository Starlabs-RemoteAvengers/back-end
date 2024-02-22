using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    internal class ApplicationUserRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string PersonalNumber { get; set; }
    }
}
