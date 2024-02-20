
namespace AppointEase.Application.Contracts.Models.Request
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
