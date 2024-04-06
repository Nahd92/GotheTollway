using GotheTollway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GotheToll.Database.Context
{
    public class GotheTollWayContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleOwner> VehicleOwners { get; set; }
        public DbSet<TollPassage> TollPassages { get; set; }
        public DbSet<TollFee> TollFee { get; set; }
        public DbSet<TollExemption> TollExemptions { get; set; }
    }
}
