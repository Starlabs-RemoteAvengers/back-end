
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Application.Contracts.Models.Request
{
    public class ClinicRequest
    {
        public ClinicRequest()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; private set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Location { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public string? OtherDetails { get; set; }
        public ICollection<DoctorRequest> Doctors { get; set; } = new List<DoctorRequest>();


    }
}
