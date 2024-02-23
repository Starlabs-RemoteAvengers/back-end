using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AppointEase.Data.Contracts.Identity;

namespace AppointEase.Data.Contracts.Models;
public partial class Patient : ApplicationUser
{
    public string Gender { get; set; }
    public string Description {  get; set; }

}
