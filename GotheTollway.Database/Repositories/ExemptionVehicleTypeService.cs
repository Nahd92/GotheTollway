using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories
{
    public class ExemptionVehicleTypeService : RepositoryBase, IExemptionVehicleTypeRepository
    {
        public ExemptionVehicleTypeService(GotheTollWayContext context) : base(context)
        {
        }

        public async Task<List<ExemptedVehicleType>> GetAllExemptedVehicleTypesAsync()
        {
            return await _context.ExemptedVehicleTypes.ToListAsync();
        }
    }
}
