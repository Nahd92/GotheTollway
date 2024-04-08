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
                                       .Include(x => x.Vehicle)
                                            .ThenInclude(x => x.Owner)
                                                .Where(x => x.Vehicle.RegistrationNumber == registrationNumber)
                                                    .ToListAsync();
        }

        public async Task<List<TollPassage>> GetAlltTollPassageWithinLastHour(string registrationNumber)
        {
           return await _context.TollPassages
                        .Where(x => x.Date > DateTime.UtcNow.AddHours(-1))
                            .Where(x => x.Vehicle.RegistrationNumber == registrationNumber)
                                .OrderBy(x => x.Date)
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

        public async Task<TollPassage> UpdateTollPassage(TollPassage tollPassage)
        {
            var tollPassageToUpdate = _context.TollPassages.Update(tollPassage).Entity;
            await _context.SaveChangesAsync();
            return tollPassageToUpdate;
        }
    }
}
