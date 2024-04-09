using CSharpFunctionalExtensions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;
using Moq;
using GotheTollway.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace GotheTollController.Tests.TollControllerTest
{
    public class TollControllerTests
    {
        private readonly Mock<ITollService> _tollService;
        private readonly TollController _tollController;

        public TollControllerTests()
        {
            _tollService = new Mock<ITollService>();
            _tollController = new TollController(_tollService.Object);
        }

        [Fact]
        public async Task HandleTollPass_WhenCalledWithValidRequest_ReturnsOk()
        {
            // Arrange
            var processTollRequest = new ProcessTollRequest
            {
                Date = DateTime.Now,
                RegistrationNumber = "ABC123"
            };

            var commandResult = new CommandResult(Result.Success());
            _tollService.Setup(x => x.ProcessToll(It.IsAny<ProcessTollRequest>())).ReturnsAsync(commandResult);


            // Act
            var result = await _tollController.HandleTollPass(processTollRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task HandleTollPass_WHenCalledWithInvalidRegistratioNumber_ReturnsBadRequest()
        {
            // Arrange
            var processTollRequest = new ProcessTollRequest
            {
                Date = DateTime.Now,
                RegistrationNumber = "ABC1A2"
            };

            // Act
            var result = await _tollController.HandleTollPass(processTollRequest);

            // Assert
            var resultMessage = result.Should().BeOfType<BadRequestObjectResult>();
            resultMessage.Subject.Value.Should().Be("Registration number must be in the format ABC123.");
        }

        [Fact]
        public async Task HandleTollPass_WhenCalledWithDateInThePast_ReturnsBadRequest()
        {
            // Arrange
            var processTollRequest = new ProcessTollRequest
            {
                Date = DateTime.Now.AddDays(-1),
                RegistrationNumber = "ABC222"
            };

            // Act
            var result = await _tollController.HandleTollPass(processTollRequest);

            // Assert
            var resultMessage = result.Should().BeOfType<BadRequestObjectResult>();
            resultMessage.Subject.Value.Should().Be("Time cannot be in the past.");
        }

        [Fact]
        public async Task HandleTollPass_WhenCalledWithMissingRegistrationNumber_ReturnsBadRequest()
        {
            // Arrange
            var processTollRequest = new ProcessTollRequest
            {
                Date = DateTime.Now,
            };

            // Act
            var result = await _tollController.HandleTollPass(processTollRequest);

            // Assert
            var resultMessage = result.Should().BeOfType<BadRequestObjectResult>();
            resultMessage.Subject.Value.Should().Be("Registration number cannot be null or empty.");
        }
    }
}
