using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AppointEase.Data.Contracts.Models
{
    public partial class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int IdClinci { get; set; }
        public string PersonalNumber { get; set; }
        public string DoctorName { get; set; }
        public string Specializations { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Clinic ClinicNavigation { get; set; } = null!;
    }
}
