using CSharpFunctionalExtensions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Helpers;
using System.Text.RegularExpressions;

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

            if (!Regex.IsMatch(processTollRequest.RegistrationNumber.ToUpper(), "^[A-Z]{3}[0-9]{2}[A-Z0-9]{1}$"))
            {
                return new CommandResult(Result.Failure("Registration number must be in the format ABC123."));
            }



            if (processTollRequest.Date < DateTime.UtcNow)
            {
                return new CommandResult(Result.Failure("Time cannot be in the past."));
            }
            
            
            return new CommandResult(Result.Success());
        }
    }
}
