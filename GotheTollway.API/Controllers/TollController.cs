using GotheTollway.API.Validations;
using GotheTollway.Contract.Requests;
using GotheTollway.Contract.Responses;
using GotheTollway.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GotheTollway.API.Controllers
{
    public class TollController(ITollService tollService) : ControllerBase
    {
        private readonly ITollService _tollService = tollService;

        [HttpPost]
        [Route("api/toll/createtollpassage")]
        public async Task<IActionResult> HandleTollPass([FromBody] ProcessTollRequest processTollRequest)
        {
            var validationResult = TollValidations.ValidateRegistrationNumber(processTollRequest);

            if(validationResult.Result.IsFailure)
            {
                return BadRequest(validationResult.Result.Error);
            }

            var commandResult = await _tollService.ProcessToll(processTollRequest);
            return commandResult.Result.IsSuccess ? Ok() : BadRequest(commandResult.Result.Error);
        }
    }
}
