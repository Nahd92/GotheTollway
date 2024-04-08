using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories
{
    public class TollFeeRepository : RepositoryBase, ITollFeeRepository
    {
        public TollFeeRepository(GotheTollWayContext context) : base(context)
        {
        }

        public async Task<List<TollFee>> GetAllTollFeesConfigs()
        {
            return await _context.TollFee.ToListAsync();
        }
    }
}
