using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollExemption
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; } 
        public DayOfWeek? ExemptedDayOfWeek { get; set; } 
        public DateTimeOffset? ExemptionStartDate { get; set; }
        public DateTimeOffset? ExemptionEndDate { get; set; } 
    }
}
