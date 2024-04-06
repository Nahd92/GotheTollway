using GotheTollway.Domain.Entities;

namespace GotheTollway.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetVehicleByRegistrationNumber(string registrationNumber);
        Task<Vehicle> CreateVehicle(Vehicle vehicle);
    }
}