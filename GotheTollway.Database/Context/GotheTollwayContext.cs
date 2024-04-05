using GotheTollway.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace GotheToll.Database.Context
{
    public class GotheTollWayContext : DbContext
    {
        public GotheTollWayContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleOwner> VehicleOwners { get; set; }
        public DbSet<TollPassage> TollPassages { get; set; }
        public DbSet<TollFee> TollFee { get; set; }
        public DbSet<TollExemption> TollExemptions { get; set; }
    }
}
