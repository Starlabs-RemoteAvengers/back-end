using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class ApplicationUserRequest
    {
        public ApplicationUserRequest()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; private set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhotoData { get; set; }
        public string? PhotoFormat { get; set; }
    }
}
