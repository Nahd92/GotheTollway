using GotheTollway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollExemption
    {
        [Key]
        public int Id { get; set; }
        // Optional name of the exemption.
        public string? Description { get; set; }
        public bool IsActive { get; set; } 
        public DayOfWeek? ExemptedDayOfWeek { get; set; } 
        public TimeSpan? ExemptionStartTime { get; set; }
        public TimeSpan? ExemptionEndTime { get; set; }
        public List<VehicleType> ExemptedVehicleTypes { get; set; } = new();
    }
}
