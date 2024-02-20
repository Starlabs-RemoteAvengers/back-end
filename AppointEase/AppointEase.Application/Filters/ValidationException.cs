namespace AppointEase.Application.Filters
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException(List<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
