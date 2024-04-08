using GotheTollway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class ExemptedVehicleType
    {
        [Key]
        public int Id{ get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
