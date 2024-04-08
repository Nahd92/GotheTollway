namespace GotheTollway.Contract.Requests
{
    public class ProcessTollRequest
    {
        public DateTimeOffset Date { get; set; }
        public string RegistrationNumber { get;     set; } = string.Empty;
    }
}
