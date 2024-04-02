using Microsoft.AspNetCore.Identity;


namespace AppointEase.Data.Contracts.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string Role { get; set; }
        public string Address {  get; set; }
        public string? PhotoData { get; set; }
        public string? PhotoFormat { get; set; }

    }

}
