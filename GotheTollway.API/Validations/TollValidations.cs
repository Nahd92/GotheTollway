using CSharpFunctionalExtensions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Helpers;

namespace GotheTollway.API.Validations
{
    public static class TollValidations
    {
        public static CommandResult ValidateRegistrationNumber(ProcessTollRequest processTollRequest)
        {
            if (processTollRequest == null)
            {
                return new CommandResult(Result.Failure("The request cannot be null"));
            }

            if (string.IsNullOrEmpty(processTollRequest.RegistrationNumber))
            {
                return new CommandResult(Result.Failure("Registration number cannot be null or empty."));
            }

            if (processTollRequest.Date < DateTime.UtcNow)
            {
                return new CommandResult(Result.Failure("Time cannot be in the past."));
            }
            
            
            return new CommandResult(Result.Success());
        }
    }
}
