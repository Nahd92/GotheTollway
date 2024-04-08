using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories;
public class TollExemptionRepository(GotheTollWayContext context) : RepositoryBase(context), ITollExemptionRepository
{
    public async Task<List<TollExemption>> GetTollExemptions()
    {
        return await _context.TollExemptions.ToListAsync();
    }
}
