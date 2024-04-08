using FluentAssertions;
using GotheTollway.Domain.Enums;
using GotheTollway.Domain.Models.VehicleAPI;
using GotheTollway.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace GotheTollway.Infrastructure.Tests.VehicleAPI
{
    public class VehicleAPIServiceTests
    {
        private readonly Mock<ILogger<VehicleAPIService>> _loggerMock;
        private readonly VehicleAPIService _service;

        public VehicleAPIServiceTests()
        {
            _loggerMock = new Mock<ILogger<VehicleAPIService>>();
            _service = new VehicleAPIService(_loggerMock.Object);
        }

        [Fact]
        public async Task GetVehicleData_WithValidRegistrationNumber_ReturnsVehicleResponse()
        {
            // Arrange
            var registrationNumber = "ABC123";

            // Act
            var result = await _service.GetVehicleData(registrationNumber);

            // Assert
            result.Should().NotBeNull();
            result.VehicleType.Should().Be(VehicleType.Car);
            result.VehicleOwnerResponse.FirstName.Should().Be("Test");
            result.VehicleOwnerResponse.LastName.Should().Be("Testsson");
        }
    }
}
