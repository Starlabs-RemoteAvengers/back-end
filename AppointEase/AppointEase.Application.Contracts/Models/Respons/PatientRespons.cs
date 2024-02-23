using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.ModelsRespond
{
    public class PatientResponse
    {
        public string Id {  get; set; } //qikjo o e shtune
        public string PersonalNumber { get; set; }
        //public string Name { get; set; }
        //public string Surname { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        //public string Address { get; set; }
        //public string ContactNumber { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
    }
}
