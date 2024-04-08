using GotheToll.Database.Context;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace GotheTollway.Database.Repositories
{
    public class TollRepository(GotheTollWayContext context) : RepositoryBase(context), ITollPassageRepository
    {
        public async Task<List<TollPassage>> GetAllTollPassagesByRegistrationNumber(string registrationNumber)
        {
            return await _context.TollPassages
                                        .Where(x => x.Vehicle.RegistrationNumber == registrationNumber)
                                            .ToListAsync();
        }

        public async Task<TollPassage?> GetTollPassageByRegistrationNumber(string registrationNumber)
        {
            return await _context
                            .TollPassages
                                .SingleOrDefaultAsync(x => x.Vehicle.RegistrationNumber == registrationNumber);
        }

        public async Task<TollPassage> CreateTollPassage(TollPassage tollPassage)
        {
            var createdTollPassage = _context.TollPassages.Add(tollPassage).Entity;
            await _context.SaveChangesAsync();
            return createdTollPassage;
        }
    }
}
