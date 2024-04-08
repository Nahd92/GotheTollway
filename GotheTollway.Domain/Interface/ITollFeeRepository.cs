using GotheTollway.Domain.Entities;

namespace GotheTollway.Domain.Interface
{
    public interface ITollFeeRepository
    {
        Task<List<TollFee>> GetAllTollFeesConfigs();
    }
}
