using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories
{
    public class VehicleRepository : RepositoryBase, IVehicleRepository
    {
        public VehicleRepository(GotheTollWayContext context) : base(context)
        {
        }

        public async Task<Vehicle> CreateVehicle(Vehicle vehicle)
        {
            var createdVehicle = _context.Add(vehicle).Entity;
            await _context.SaveChangesAsync();
            return createdVehicle;  
        }

        public async Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationNumber)
        {
            return await _context.Vehicles.SingleOrDefaultAsync(v => v.RegistrationNumber == registrationNumber);
        }
    }
}
