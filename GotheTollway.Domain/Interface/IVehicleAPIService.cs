using GotheTollway.Domain.Models.VehicleAPI;

namespace GotheTollway.Domain.Interface
{
    public interface IVehicleAPIService
    {
        Task<VehicleResponse> GetVehicleData(string registrationNumber);
    }
}
