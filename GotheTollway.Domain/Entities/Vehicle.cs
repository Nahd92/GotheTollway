using GotheTollway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;

        public VehicleType VehicleType { get; set; }
        public VehicleOwner Owner { get; set; } = new();
        public List<TollPassage> TollPassages { get; set; } = new();
    }
}
