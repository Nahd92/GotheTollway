using GotheTollway.Contract.Responses;
using GotheTollway.Domain.Helpers;

namespace GotheTollway.Domain.Interface
{
    public interface IInvoiceService
    {
        Task<CommandResult> CreateInvoice(string registrationNumber);
    }
}