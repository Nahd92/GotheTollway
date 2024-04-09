using FluentAssertions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Enums;
using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Models.VehicleAPI;
using GotheTollway.Domain.Repositories;
using GotheTollway.Domain.Services;
using Moq;

namespace GotheTollway.Domain.Tests.Services
{
    public class TollServiceTests
    {
        private readonly Mock<ITollPassageRepository> _tollPassageRepository;
        private readonly Mock<IVehicleAPIService> _vehicleAPIService;
        private readonly Mock<IVehicleRepository> _vehicleRepository;
        private readonly Mock<ITollExemptionRepository> _tollExemptionRepository;
        private readonly Mock<ITollFeeRepository> _tollFeeRepository;
        private readonly Mock<IExemptionVehicleTypeRepository> _exemptionVehicleTypesRepo;

        private readonly TollService _tollService;

        public TollServiceTests()
        {
            _tollPassageRepository = new Mock<ITollPassageRepository>();
            _vehicleAPIService = new Mock<IVehicleAPIService>();
            _vehicleRepository = new Mock<IVehicleRepository>();
            _tollExemptionRepository = new Mock<ITollExemptionRepository>();
            _tollFeeRepository = new Mock<ITollFeeRepository>();
            _exemptionVehicleTypesRepo = new Mock<IExemptionVehicleTypeRepository>();

            _tollService = new TollService(
                _tollPassageRepository.Object,
                _vehicleAPIService.Object,
                _vehicleRepository.Object,
                _tollExemptionRepository.Object,
                _tollFeeRepository.Object,
                _exemptionVehicleTypesRepo.Object);
        }

        private readonly ProcessTollRequest request = new()
        {
            RegistrationNumber = "ABC123",
            Date = new DateTime(2024, 4, 7, 6, 15, 0)
        };

        private void SetupDefaultMock()
        {
            _vehicleRepository.Setup(x => x.GetVehicleByRegistrationNumber(request.RegistrationNumber))
              .ReturnsAsync(new Vehicle
              {
                  RegistrationNumber = request.RegistrationNumber,
                  VehicleType = VehicleType.Military
              });

            _tollPassageRepository.Setup(x => x.GetAllTollPassagesByRegistrationNumber(request.RegistrationNumber))
                        .ReturnsAsync(new List<TollPassage>());


            _tollPassageRepository.Setup(x => x.GetAlltTollPassageWithinLastHour(It.IsAny<string>()))
                .ReturnsAsync(new List<TollPassage>());

            _vehicleAPIService.Setup(x => x.GetVehicleData(request.RegistrationNumber))
                .ReturnsAsync(new VehicleResponse
                {
                    RegistrationNumber = "ABC123",
                    VehicleType = VehicleType.Car,
                    VehicleOwnerResponse = new VehicleOwnerResponse
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Address = "123 Main Street"
                    }
                });

            _tollFeeRepository.Setup(x => x.GetAllTollFeesConfigs())
                .ReturnsAsync(
                    [
                        new TollFee
                        {
                            StartTime = new TimeSpan(6, 0, 0),
                            EndTime = new TimeSpan(6, 29, 0),
                            Fee = 10
                        }
                    ]);

            _exemptionVehicleTypesRepo.Setup(x => x.GetAllExemptedVehicleTypesAsync())
                .ReturnsAsync([]);

        }

        [Fact]
        public async Task ProcessToll_ExemptedVehicle_NoFeeCharged()
        {
            // Arrange
            SetupDefaultMock();
            _vehicleRepository.Setup(x => x.GetVehicleByRegistrationNumber(request.RegistrationNumber))
                .ReturnsAsync(new Vehicle
                {
                    RegistrationNumber = request.RegistrationNumber,
                    VehicleType = VehicleType.Military
                });

            _exemptionVehicleTypesRepo.Setup(x => x.GetAllExemptedVehicleTypesAsync())
                .ReturnsAsync(
                [
                    new() {
                       VehicleType = VehicleType.Military
                    }
                ]);

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Result.IsSuccess.Should().BeTrue();
            _tollPassageRepository.Verify(x => x.CreateTollPassage(It.IsAny<TollPassage>()), Times.Once);
        }

        [Fact]
        public async Task ProcessToll_NonExemptedVehicle_CorrectFeeCharged()
        {
            // Arrange 
            SetupDefaultMock();
            _tollExemptionRepository.Setup(x => x.GetTollExemptions())
                .ReturnsAsync([]);

            _tollFeeRepository.Setup(x => x.GetAllTollFeesConfigs())
                .ReturnsAsync(
                [
                    new TollFee
                    {
                        StartTime = new TimeSpan(6, 0, 0),
                        EndTime = new TimeSpan(6, 29, 0),
                        Fee = 10
                    }
                ]);

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Result.IsSuccess.Should().BeTrue();
            _tollPassageRepository.Verify(x => x.CreateTollPassage(It.Is<TollPassage>(passage => passage.Fee == 10)), Times.Once);
        }

        [Fact]
        public async Task ProcessToll_ExemptionTimeRange_CorrectFeeCharged()
        {
            // Arrange
            SetupDefaultMock();
            var request = new ProcessTollRequest
            {
                RegistrationNumber = "ABC123",
                Date = new DateTime(2024, 4, 1, 20, 00, 0)
            };

            _tollExemptionRepository.Setup(x => x.GetTollExemptions())
                .ReturnsAsync(new List<TollExemption>
                {
                    new TollExemption
                    {
                        ExemptedDayOfWeek = DayOfWeek.Monday,
                        ExemptionStartTime = TimeSpan.FromHours(18.30),
                        ExemptionEndTime = TimeSpan.FromHours(05.59)
                    }
                });

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Result.IsSuccess.Should().BeTrue();
            _tollPassageRepository.Verify(x => x.CreateTollPassage(It.Is<TollPassage>(x => x.Fee == 0)), Times.Once);
        }

        [Fact]
        public async Task HandleVehicleData_VehicleDoesNotExistInDatabase_ReturnsVehicleFromExternalAPI()
        {
            // Arrange
            SetupDefaultMock();
            _vehicleRepository.Setup(x => x.GetVehicleByRegistrationNumber(request.RegistrationNumber))
                .ReturnsAsync((Vehicle)null);

            _tollPassageRepository.Setup(x => x.GetAllTollPassagesByRegistrationNumber(request.RegistrationNumber))
                .ReturnsAsync([]);

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Should().NotBeNull();
            _vehicleAPIService.Verify(x => x.GetVehicleData(It.IsAny<string>()), Times.Once);
            _vehicleRepository.Verify(x => x.CreateVehicle(It.IsAny<Vehicle>()), Times.Once);
        }

        [Fact]
        public async Task HandleVehicleData_VehicleExistsInDatabase_ReturnsVehicleFromDatabase()
        {
            // Arrange
            SetupDefaultMock();

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Should().NotBeNull();
            _vehicleAPIService.Verify(x => x.GetVehicleData(It.IsAny<string>()), Times.Never);
            _vehicleRepository.Verify(x => x.CreateVehicle(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task CheckExemptedVehicleTollFee_ExemptedDayOfWeek_VehicleExemptedFromTollFee()
        {
            // Arrange
            SetupDefaultMock();
            _tollExemptionRepository.Setup(x => x.GetTollExemptions())
                .ReturnsAsync(new List<TollExemption>
                {
                    new TollExemption
                    {
                        ExemptedDayOfWeek = DayOfWeek.Sunday,
                        ExemptionStartTime = TimeSpan.FromHours(8),
                        ExemptionEndTime = TimeSpan.FromHours(10)
                    }
                });

            _tollFeeRepository.Setup(x => x.GetAllTollFeesConfigs())
                .ReturnsAsync(
                [
                    new TollFee
                    {
                        StartTime = new TimeSpan(6, 0, 0),
                        EndTime = new TimeSpan(6, 29, 0),
                        Fee = 0,
                    }
                ]);

            // Act
            var result = await _tollService.ProcessToll(request);

            // Assert
            result.Result.IsSuccess.Should().BeTrue();
            _tollPassageRepository.Verify(x => x.CreateTollPassage(It.Is<TollPassage>(x => x.Fee == 0)), Times.Once);
        }
    }
}
