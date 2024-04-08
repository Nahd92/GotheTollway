using CSharpFunctionalExtensions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Repositories;

namespace GotheTollway.Domain.Services
{
    public class TollService(ITollPassageRepository tollRepository,
                             IVehicleAPIService vehicleAPIService,
                             IVehicleRepository vehicleRepository,
                             ITollExemptionRepository tollExemptionRepository,
                             ITollFeeRepository tollFeeRepository) : ITollService
    {
        private readonly ITollPassageRepository _tollRepository = tollRepository;
        private readonly IVehicleAPIService _vehicleAPIService = vehicleAPIService;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ITollExemptionRepository _tollExemptionRepository = tollExemptionRepository;
        private readonly ITollFeeRepository _tollFeeRepository = tollFeeRepository;

        public async Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest)
        {
            // 1. Get the vehicle data from database if exists else external API
            var vehicle = await HandleVehicleData(processTollRequest);

            if (vehicle == null)
            {
                return new CommandResult(Result.Failure("Vehicle not found"));
            }

            // 2. Get all the passages for the vehicle by registration number
            var tollPassages = await _tollRepository.GetAllTollPassagesByRegistrationNumber(processTollRequest.RegistrationNumber);

            // 3. Get All the toll exemptions   
            var tollFeesExemptions = await _tollExemptionRepository.GetTollExemptions();

            // 4. Check if the vehicle is exempted from toll fee
            var vehicleExempted = await CheckExemptedVehicleTollFee(tollFeesExemptions, vehicle, processTollRequest);

            if (vehicleExempted.Result.IsSuccess)
            {
                return vehicleExempted;
            }

            // Get the last passage within the last hour
            var lastPassage = tollPassages
                    .Where(x => x.Date > DateTime.UtcNow.AddHours(-1))
                    .OrderBy(x => x.Date) 
                    .FirstOrDefault();

            // Check if the vehicle has passed the toll within the last hour
            if (lastPassage != null && processTollRequest.Time <= lastPassage.Date.AddHours(1))
            {
                    await _tollRepository.CreateTollPassage(new TollPassage
                    {
                        Fee = 0,
                        Date = processTollRequest.Time,
                        Vehicle = vehicle,
                    });

                    return new CommandResult(Result.Success());
            }

            // 6. Get the fee for the toll passage
            await GetAndAppendFeeToTollPassage(processTollRequest, vehicle);

            return new CommandResult(Result.Success());
        }

        private async Task GetAndAppendFeeToTollPassage(ProcessTollRequest processTollRequest, Vehicle vehicle)
        {
            var allFeeConfigs = await _tollFeeRepository.GetAllTollFeesConfigs();

            var fee = allFeeConfigs
                            .FirstOrDefault(x =>
                                    x.StartTime <= processTollRequest.Time.TimeOfDay &&
                                    x.EndTime >= processTollRequest.Time.TimeOfDay);

            if (fee == null)
            {
                throw new Exception("No fee rule found for the time of day");
            }

            await _tollRepository.CreateTollPassage(new TollPassage
            {
                Date = processTollRequest.Time,
                Vehicle = vehicle,
                Fee = fee.Fee
            });
        }

        /// <summary>
        /// This methods checks if the vehicle is exempted from toll fee
        /// if the vehicle is exempted from toll fee, it will create a toll passage
        /// but will not charge the vehicle, it will return a message that the vehicle is exempted from toll fee.
        /// </summary>
        /// <param name="exemptions"></param>
        /// <param name="vehicle"></param>
        private async Task<CommandResult> CheckExemptedVehicleTollFee(List<TollExemption> exemptions, Vehicle vehicle, ProcessTollRequest processTollRequest)
        {
            if(exemptions != null)
            {
                if (exemptions.Any(x => x.ExemptedVehicleTypes.Contains(vehicle.VehicleType)))
                {
                    var exemptedTollFee = exemptions.FirstOrDefault(x => x.ExemptedVehicleTypes.Contains(vehicle.VehicleType));

                    if (exemptedTollFee != null)
                    {
                        await _tollRepository.CreateTollPassage(new TollPassage
                        {
                            Date = processTollRequest.Time,
                            Vehicle = vehicle,
                            Fee = 0
                        });

                        return new CommandResult(Result.Success());
                    }
                }

                /// Check if the time falls within exemption time range
                if (exemptions.Any(e => e.ExemptionStartTime.HasValue && e.ExemptionEndTime.HasValue))
                {
                    foreach (var exemption in exemptions)
                    {
                        var timeOfDay = processTollRequest.Time.TimeOfDay;
                        if (timeOfDay >= exemption.ExemptionStartTime && timeOfDay < exemption.ExemptionEndTime)
                        {
                            await _tollRepository.CreateTollPassage(new TollPassage
                            {
                                Date = processTollRequest.Time,
                                Vehicle = vehicle,
                                Fee = 0
                            });

                            return new CommandResult(Result.Success());
                        }
                    }
                }

                // Check if the vehicle is exempted from toll fee based on the day of the week
                if (exemptions != null && exemptions.Any(x => x.ExemptedDayOfWeek == processTollRequest.Time.DayOfWeek))
                {
                    var exemptedTollFee = exemptions.FirstOrDefault(x => x.ExemptedDayOfWeek == processTollRequest.Time.DayOfWeek);

                    if (exemptedTollFee != null)
                    {
                        await _tollRepository.CreateTollPassage(new TollPassage
                        {
                            Date = processTollRequest.Time,
                            Vehicle = vehicle,
                            Fee = 0
                        });

                        return new CommandResult(Result.Success());
                    }
                }
            }

            return new CommandResult(Result.Failure("process doesn't fall within any exemptions"));
        }

        private async Task<Vehicle?> HandleVehicleData(ProcessTollRequest processTollRequest)
        {
            var vehicleEntity = await _vehicleRepository.GetVehicleByRegistrationNumber(processTollRequest.RegistrationNumber);

            // If vehicleEntity is null we need to get the vehicle data from the external API
            // togheter with the owner details and vehicle type
            if (vehicleEntity == null)
            {
                var vehicleData = await _vehicleAPIService.GetVehicleData(processTollRequest.RegistrationNumber);

                if (vehicleData == null)
                {
                    return null;
                }

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
