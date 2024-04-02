namespace AppointEase.Application.Contracts.Models.Request
{
    public class MessageRequest
    {

        public string? MessageId { get; set; } = Guid.NewGuid().ToString();
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
