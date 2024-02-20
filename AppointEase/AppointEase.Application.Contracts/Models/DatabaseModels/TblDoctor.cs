using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Models.DbModels
{
    public partial class TblDoctor
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



        public virtual TblClinic ClinicNavigation { get; set; } = null!;
    }
}
