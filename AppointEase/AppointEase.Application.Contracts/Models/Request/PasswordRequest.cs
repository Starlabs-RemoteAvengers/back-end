namespace AppointEase.Application.Contracts.Models.Request
{
    public class PasswordRequest
    {
        public string UserId { get; set; }
        public string? NewPassword { get; set; }

        public string? OldPassword { get; set; }

    }
}
