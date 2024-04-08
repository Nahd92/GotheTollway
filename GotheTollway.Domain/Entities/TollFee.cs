using GotheTollway.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollFee
    {
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal Fee { get; set; }
    }
}
