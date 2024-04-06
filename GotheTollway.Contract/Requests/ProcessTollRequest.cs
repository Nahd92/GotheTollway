namespace GotheTollway.Contract.Requests
{
    public class ProcessTollRequest
    {
        public DateTimeOffset Time { get; set; }
        public string? Description { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
    }
}
