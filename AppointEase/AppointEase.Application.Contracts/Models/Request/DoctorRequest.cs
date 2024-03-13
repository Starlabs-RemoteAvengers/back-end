using System;
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class DoctorRequest
    {
        public DoctorRequest()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public int PersonalNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Specialisation { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ClinicId { get; set; }
        //Picture
        public string? PhotoData { get; set; }  
        public string? PhotoFormat { get; set; }
        public string? Description { get; set; }
    }
}
