using CSharpFunctionalExtensions;
using GotheTollway.Contract.Responses;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace GotheTollway.Domain.Services
{
    public class InvoiceService(ILogger<InvoiceService> logger, ITollPassageRepository tollPassage) : IInvoiceService
    {
        private readonly ITollPassageRepository _tollPassage = tollPassage;
        private readonly ILogger<InvoiceService> _logger = logger;

        public async Task<CommandResult> CreateInvoice(string registrationNumber)
        {
            var passages = await _tollPassage.GetAllTollPassagesByRegistrationNumber(registrationNumber);
            var passageInfo = passages.FirstOrDefault();

            if (passageInfo == null)
            {
                _logger.LogError("No passages found for the vehicle with registration number {registrationNumber}", registrationNumber);
                return new CommandResult(Result.Failure("No passages found for the vehicle with registration number {registrationNumber}"));
            }

            return new CommandResult(Result.Success(), new InvoiceResponse
            {
                Generated = DateTime.Now,
                TotalSum = passages.Sum(p => p.Fee),
                Vehicle = new VehicleResponse
                {
                    RegistrationNumber = passageInfo.Vehicle.RegistrationNumber,
                    VehicleType = passageInfo.Vehicle.VehicleType.ToString(),
                },
                VehicleOwner = new VehicleOwnerResponse
                {
                    FirstName = passageInfo.Vehicle.Owner.FirstName,
                    LastName = passageInfo.Vehicle.Owner.LastName,
                    ZipCode = passageInfo.Vehicle.Owner.ZipCode,
                    Address = passageInfo.Vehicle.Owner.Address,
                }
            });
        }
    }
}
