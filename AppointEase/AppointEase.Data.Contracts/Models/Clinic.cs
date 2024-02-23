using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Data.Contracts.Models
{
    public partial class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
