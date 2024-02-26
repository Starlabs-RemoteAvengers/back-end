namespace AppointEase.Application.Contracts.Models.Request
{
    public class AdminRequest
    {
        public int PersonalNumber {  get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
