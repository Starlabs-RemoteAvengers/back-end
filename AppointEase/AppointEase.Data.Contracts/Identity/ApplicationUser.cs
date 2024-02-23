using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Identity
{
    public class ApplicationUser : IdentityUser
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string PersonalNumber { get; set; }
    }
}
