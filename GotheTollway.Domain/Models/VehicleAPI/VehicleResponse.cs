using GotheTollway.Domain.Enums;
using System.Text.Json.Serialization;

namespace GotheTollway.Domain.Models.VehicleAPI
{
    public class VehicleResponse
    {
        [JsonPropertyName("registrationNumber")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [JsonPropertyName("owner")]
        public VehicleOwnerResponse VehicleOwnerResponse { get; set; } = new();

        public VehicleType VehicleType { get; set; } 
    }

    public class VehicleOwnerResponse
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}
