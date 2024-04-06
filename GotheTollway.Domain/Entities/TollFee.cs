using GotheTollway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollFee
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public decimal Fee { get; set; }
        public List<VehicleType> ExemptedVehicleTypes { get; set; } = [];
        public List<TollPassage> TollPassages { get; set; } = [];
    }
}
