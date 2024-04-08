using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Contract.Requests
{
    public class ProcessTollRequest
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string RegistrationNumber { get;     set; } = string.Empty;
    }
}
