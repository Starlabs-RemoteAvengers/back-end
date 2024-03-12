using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AppointEase.Data.Contracts.Identity;


namespace AppointEase.Data.Contracts.Models
{
    public partial class Doctor : ApplicationUser
    {
        public int PersonalNumber { get; set; }
        public string? Specialisation { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Description { get; set; }
        public string ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}
