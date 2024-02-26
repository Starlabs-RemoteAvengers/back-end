using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AppointEase.Data.Contracts.Identity;

namespace AppointEase.Data.Contracts.Models;

public partial class Admin : ApplicationUser
{
    public string? Test { get; set; }
    public int PersonalNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}
