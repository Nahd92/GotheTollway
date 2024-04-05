using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;

namespace GotheTollway.Domain.Services
{
    public class TollService : ITollService
    {


        public Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest)
        {
            throw new NotImplementedException();
        }
    }
}
