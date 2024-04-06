using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories
{
    public class TollRepository(GotheTollWayContext context) : RepositoryBase(context), ITollRepository
    {
        public async Task<List<TollPassage?>> GetAllTollPassagesByRegistrationNumber(string registrationNumber)
        {
            return await _context.TollPassages
                                    .Include(x => x.TollFee).Where(x => x.Vehicle.RegistrationNumber == registrationNumber).ToListAsync();
        }

        public async Task<TollPassage?> GetTollPassageByRegistrationNumber(string registrationNumber)
        {
            return await _context
                            .TollPassages
                                .Include(x => x.TollFee)
                                .SingleOrDefaultAsync(x => x.Vehicle.RegistrationNumber == registrationNumber);
        }

        public Task<TollPassage> HandleTollPassage(TollPassage tollPassage)
        {
            throw new NotImplementedException();
        }
    }
}
