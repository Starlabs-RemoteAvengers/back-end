using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.ModelsDto
{
    public class ClinicRequest
    {
        public string ClinicName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
