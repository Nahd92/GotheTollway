using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Repositories;

namespace GotheTollway.Domain.Services
{
    public class TollService(ITollRepository tollRepository, IVehicleAPIService vehicleAPIService, IVehicleRepository vehicleRepository) : ITollService
    {
        private readonly ITollRepository _tollRepository = tollRepository;
        private readonly IVehicleAPIService _vehicleAPIService = vehicleAPIService;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        public async Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest)
        {
            // 1. Get the vehicle data from the external API
            // create a new vehicle if it does not exist else return the existing vehicle
            var vehicle = await HandleVehicleData(processTollRequest);

            // 2. Get the toll passage by vehicle id and toll id
            var tollPassages = await _tollRepository.GetAllTollPassagesByRegistrationNumber(processTollRequest.RegistrationNumber);


        }

        private async Task<Vehicle> HandleVehicleData(ProcessTollRequest processTollRequest)
        {
            var vehicleEntity = await _vehicleRepository.GetVehicleByRegistrationNumber(processTollRequest.RegistrationNumber);

            // If vehicleEntity is null we need to get the vehicle data from the external API
            // togheter with the owner details and vehicle type
            if (vehicleEntity == null)
            {
                var vehicleData = await _vehicleAPIService.GetVehicleData(processTollRequest.RegistrationNumber);

                var createVehicle = new Vehicle
                {
                    RegistrationNumber = vehicleData.RegistrationNumber,
                    VehicleType = vehicleData.VehicleType,
                    Owner = new VehicleOwner
                    {
                        FirstName = vehicleData.VehicleOwnerResponse.FirstName,
                        LastName = vehicleData.VehicleOwnerResponse.LastName,
                        Address = vehicleData.VehicleOwnerResponse.Address
                    }
                };

                vehicleEntity = await _vehicleRepository.CreateVehicle(createVehicle);
            }

            return vehicleEntity;
        }
    }
}
