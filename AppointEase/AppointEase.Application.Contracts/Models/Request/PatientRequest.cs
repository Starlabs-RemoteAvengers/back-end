
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class PatientRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName {  get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "PersonalNumber is required.")]
        public string PersonalNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
    }
}
