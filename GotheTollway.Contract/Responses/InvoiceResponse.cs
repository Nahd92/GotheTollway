namespace GotheTollway.Contract.Responses
{
    public class InvoiceResponse
    {
        public VehicleOwnerResponse VehicleOwner { get; set; } = new();
        public VehicleResponse Vehicle { get; set; } = new();
        public decimal TotalSum { get; set; }
        public DateTime Generated { get; set; }
    }

    public class VehicleResponse
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
    }
    
    public class VehicleOwnerResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
