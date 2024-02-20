using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.ModelsDto
{
    public class DoctorRequest
    {
        public int IdClinci { get; set; }
        public string PersonalNumber { get; set; }
        public string DoctorName { get; set; }
        public string Specializations { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
