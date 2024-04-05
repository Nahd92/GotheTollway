using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Helpers;

namespace GotheTollway.Domain.Interface
{
    public interface ITollService
    {
        Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest);
    }
}