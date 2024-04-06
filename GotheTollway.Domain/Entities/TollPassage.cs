using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollPassage
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Vehicle Vehicle { get; set; } = new();
        public TollFee TollFee { get; set; } = new();
    }
}
