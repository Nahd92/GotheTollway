using GotheTollway.Domain.Entities;

namespace GotheTollway.Domain.Interface
{
    public interface IExemptionVehicleTypeRepository
    {
        Task<List<ExemptedVehicleType>> GetAllExemptedVehicleTypesAsync();
    }
}
