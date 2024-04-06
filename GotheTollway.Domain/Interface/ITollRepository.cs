using GotheTollway.Domain.Entities;

namespace GotheTollway.Domain.Interface
{
    public interface ITollRepository
    {
        Task<List<TollPassage?>> GetAllTollPassagesByRegistrationNumber(string registrationNumber);
        Task<TollPassage?> GetTollPassageByRegistrationNumber(string registrationNumber);
        Task<TollPassage> HandleTollPassage(TollPassage tollPassage);
    }
}
