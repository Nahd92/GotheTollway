using GotheTollway.Contract.Requests;
using GotheTollway.Contract.Responses;
using GotheTollway.Domain.Helpers;

namespace GotheTollway.Domain.Interface
{
    public interface ITollService
    {
        Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest);
    }
}