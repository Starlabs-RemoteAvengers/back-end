using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AppointEase.Data.Contracts.Identity;

namespace AppointEase.Data.Contracts.Models
{
    public partial class Clinic : ApplicationUser
    {
        public string? Location { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public string? OtherDetails {  get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
