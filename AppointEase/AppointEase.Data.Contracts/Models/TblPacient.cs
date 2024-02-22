using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppointEase.Data.Contracts.Models;
public partial class TblPacient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    public string PersonalNumber { get; set; } = null!;
    public string Username { get; set; } 

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ContactNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

}
