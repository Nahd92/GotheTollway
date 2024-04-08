using GotheTollway.Domain.Entities;

namespace GotheTollway.Domain.Interface
{
    public interface ITollPassageRepository
    {
        Task<List<TollPassage>> GetAllTollPassagesByRegistrationNumber(string registrationNumber);
        Task<TollPassage?> GetTollPassageByRegistrationNumber(string registrationNumber);
        Task<TollPassage> CreateTollPassage(TollPassage tollPassage);

        Task<TollPassage> UpdateTollPassage(TollPassage tollPassage);
        Task<List<TollPassage>> GetAlltTollPassageWithinLastHour(string registrationNumber);
    }
}
