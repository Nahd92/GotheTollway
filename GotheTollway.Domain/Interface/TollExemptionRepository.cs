using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Enums;

namespace GotheTollway.Domain.Interface
{
    public interface ITollExemptionRepository
    {
        Task<List<TollExemption>> GetTollExemptions();
    }
}
