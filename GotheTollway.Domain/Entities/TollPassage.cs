using System.ComponentModel.DataAnnotations;

namespace GotheTollway.Domain.Entities
{
    public class TollPassage
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public Vehicle Vehicle { get; set; } = new();

        public decimal Fee { get; set; }
    }
}
