namespace GotheTollway.Contract.Responses
{
    public class InvoiceResponse
    {
        public VehicleOwnerResponse VehicleOwner { get; set; } = new();
        public VehicleResponse Vehicle { get; set; } = new();
        public decimal TotalSum { get; set; }
        public DateTime Generated { get; set; }
        public TollPassageBaseResponse Passages { get; set; } = new();
    }

    public class TollPassageBaseResponse
    {
        public Dictionary<DateTime, List<TollPassagesResponse>> PassagesPerDay { get; set; } = new();
        public decimal TotalSum { get; set; }
    }
 
    public class TollPassagesResponse
    {
        public DateTimeOffset Date { get; set; }
        public decimal Fee { get; set; }
    }

    public class VehicleResponse
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
    }

    public class VehicleOwnerResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; } 
        public string? Address { get; set; }  
        public string? ZipCode { get; set; }  
    }
}
