using Microsoft.EntityFrameworkCore;


namespace GotheToll.Database.Context
{
    public class GotheTollContext : DbContext
    {
        protected GotheTollContext(DbContextOptions options) : base(options)
        {
        }
    }
}
