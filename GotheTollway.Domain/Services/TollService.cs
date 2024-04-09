using CSharpFunctionalExtensions;
using GotheTollway.Contract.Requests;
using GotheTollway.Domain.Entities;
using GotheTollway.Domain.Enums;
using GotheTollway.Domain.Helpers;
using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Repositories;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GotheTollway.Domain.Services
{
    public class TollService(ITollPassageRepository tollRepository,
                             IVehicleAPIService vehicleAPIService,
                             IVehicleRepository vehicleRepository,
                             ITollExemptionRepository tollExemptionRepository,
                             ITollFeeRepository tollFeeRepository,
                             IExemptionVehicleTypeRepository exemptionVehicleType,
                             ILogger<TollService> logger) : ITollService
    {
        private readonly ITollPassageRepository _tollRepository = tollRepository;
        private readonly IVehicleAPIService _vehicleAPIService = vehicleAPIService;
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly ITollExemptionRepository _tollExemptionRepository = tollExemptionRepository;
        private readonly ITollFeeRepository _tollFeeRepository = tollFeeRepository;
        private readonly IExemptionVehicleTypeRepository _exemptionVehicleType = exemptionVehicleType;
        private readonly ILogger<TollService> _logger = logger;

        public async Task<CommandResult> ProcessToll(ProcessTollRequest processTollRequest)
        {
            var allExistingVehicleTypes = await _exemptionVehicleType.GetAllExemptedVehicleTypesAsync();

            var vehicle = await HandleVehicleData(processTollRequest, allExistingVehicleTypes);

            if (vehicle == null)
            {
                return new CommandResult(Result.Failure("Vehicle not found"));
            }

            var tollFeesExemptions = await _tollExemptionRepository.GetTollExemptions();

            var vehicleExempted = await CheckExemptedVehicleTollFee(tollFeesExemptions, vehicle, processTollRequest,allExistingVehicleTypes);

            if (vehicleExempted.Result.IsSuccess)
            {
                _logger.LogInformation("Vehicle with registration number {RegistrationNumber} is exempted from toll fee.", processTollRequest.RegistrationNumber);
                return vehicleExempted;
            }

            await GetAndAppendFeeToTollPassage(processTollRequest, vehicle);

            _logger.LogInformation("Toll passage created successfully for vehicle with registration number: {RegistrationNumber}", processTollRequest.RegistrationNumber);
            return new CommandResult(Result.Success());
        }

        private async Task GetAndAppendFeeToTollPassage(ProcessTollRequest processTollRequest, Vehicle vehicle)
        {
            _logger.LogInformation("Fetching all toll fee configurations and passages for vehicle with registration number: {RegistrationNumber}", processTollRequest.RegistrationNumber);
            var allFeeConfigs = await _tollFeeRepository.GetAllTollFeesConfigs();

            _logger.LogInformation("Fetching all toll passages for vehicle with registration number: {RegistrationNumber}", processTollRequest.RegistrationNumber);
            var allPassages = await _tollRepository.GetAllTollPassagesByRegistrationNumber(processTollRequest.RegistrationNumber);

            // Extract request date
            var requestDate = processTollRequest.Date;

            // Find applicable fee configuration
            var fee = allFeeConfigs.FirstOrDefault(x =>
                            x.StartTime <= requestDate.TimeOfDay &&
                            x.EndTime >= requestDate.TimeOfDay);

            if (fee == null)
            {
                _logger.LogError("No fee rule found for the time of day.");
                throw new Exception("No fee rule found for the time of day");
            }

            // Check if there's already a passage for today with total fee 60
            var todayPassages = allPassages.Where(p => p.Date.Date == requestDate.Date);
            if (todayPassages.Any() && todayPassages.Sum(p => p.Fee) == 60)
            {

                // If fee sum for today is 60, create a passage with 0 fee
                _logger.LogInformation("Creating toll passage with fee 0 for vehicle with registration number: {RegistrationNumber}", processTollRequest.RegistrationNumber);
                await _tollRepository.CreateTollPassage(new TollPassage
                {
                    Date = requestDate,
                    Vehicle = vehicle,
                    Fee = 0
                });
            }
            else
            {
                // Otherwise, create a passage with the calculated fee
                _logger.LogInformation("Creating toll passage with fee {Fee} for vehicle with registration number: {RegistrationNumber}", fee.Fee, processTollRequest.RegistrationNumber);
                await _tollRepository.CreateTollPassage(new TollPassage
                {
                    Date = requestDate,
                    Vehicle = vehicle,
                    Fee = fee.Fee
                });
            }

            // Process the most expensive toll within the same hour
            await ProcessMostExpensiveTollWithinHour(processTollRequest);
        }


        /// <summary>
        /// This method checks if the vehicle has passed the toll within the last hour
        /// If it have passed the toll within the last hour, it will take the passage with the highest fee
        /// And charge the vehicle for that passage only.
        /// </summary>
        /// <param name="processTollRequest"></param>
        private async Task ProcessMostExpensiveTollWithinHour(ProcessTollRequest processTollRequest)
        {
            _logger.LogInformation("Checking if the vehicle has passed the toll within the last hour.");
            var allPassagesWithinLastHour = await _tollRepository.GetAlltTollPassageWithinLastHour(processTollRequest.RegistrationNumber);
            var lastPassage = allPassagesWithinLastHour.OrderBy(x => x.Date).FirstOrDefault();

            // Check if the vehicle has passed the toll within the last hour
            if (lastPassage != null && processTollRequest.Date <= lastPassage.Date.AddHours(1))
            {
                _logger.LogInformation("Passage found within the last hour. Charging the vehicle for the passage with the highest fee.");
                var passageWithHighestFee = allPassagesWithinLastHour.OrderByDescending(p => p.Fee).FirstOrDefault();

                foreach (var passage in allPassagesWithinLastHour)
                {
                    if (passage != passageWithHighestFee)
                    {
                        passage.Fee = 0;
                        await _tollRepository.UpdateTollPassage(passage);
                    }
                }
            }
        }

        /// <summary>
        /// This methods checks if the vehicle is exempted from toll fee
        /// if the vehicle is exempted from toll fee, it will create a toll passage
        /// but will not charge the vehicle, it will return a message that the vehicle is exempted from toll fee.
        /// </summary>
        /// <param name="exemptions"></param>
        /// <param name="vehicle"></param>
        private async Task<CommandResult> CheckExemptedVehicleTollFee(List<TollExemption> exemptions,
                                                                      Vehicle vehicle,
                                                                      ProcessTollRequest processTollRequest,
                                                                      List<ExemptedVehicleType> exemptedVehicleTypes)
        {
            if(exemptions != null)
            {
                    if (exemptedVehicleTypes.Any(x => x.VehicleType.Equals(vehicle.VehicleType)))
                    {
                        var exemptedTollFee = exemptedVehicleTypes.FirstOrDefault(x => x.VehicleType.Equals(vehicle.VehicleType));

                        if (exemptedTollFee != null)
                        {
                            _logger.LogInformation("Vehicle with Id {VehicleId} is exempted from toll fee. Creating toll passage with fee 0.", vehicle.Id);

                            await _tollRepository.CreateTollPassage(new TollPassage
                            {
                                Date = processTollRequest.Date,
                                Vehicle = vehicle,
                                Fee = 0
                            });

                            _logger.LogInformation("Toll passage created successfully for exempted vehicle.");
                            return new CommandResult(Result.Success());
                         }
                    }

                //Check if the time falls wihtin exemption period range

                if (exemptions.Any(e => e.ExemptionStartPeriod.HasValue && e.ExemptionEndPeriod.HasValue))
                {
                    if (exemptions.Any(e => processTollRequest.Date >= e.ExemptionStartPeriod && processTollRequest.Date <= e.ExemptionEndPeriod))
                    {
                        _logger.LogInformation("Vehicle with Id {VehicleId} is exempted from toll fee. Creating toll passage with fee 0.", vehicle.Id);

                        await _tollRepository.CreateTollPassage(new TollPassage
                        {
                            Date = processTollRequest.Date,
                            Vehicle = vehicle,
                            Fee = 0
                        });

                        _logger.LogInformation("Toll passage created successfully for exempted vehicle.");
                        return new CommandResult(Result.Success());
                    }
                }

                // Check if the vehicle is exempted from toll fee based on the day of the week
                if (exemptions != null && exemptions.Any(x => x.ExemptedDayOfWeek == processTollRequest.Date.DayOfWeek))
                {
                    var exemptedTollFee = exemptions.FirstOrDefault(x => x.ExemptedDayOfWeek == processTollRequest.Date.DayOfWeek);

                    if (exemptedTollFee != null)
                    {
                        _logger.LogInformation("Vehicle with Id {VehicleId} is exempted from toll fee. Creating toll passage with fee 0.", vehicle.Id);

                        await _tollRepository.CreateTollPassage(new TollPassage
                        {
                            Date = processTollRequest.Date,
                            Vehicle = vehicle,
                            Fee = 0
                        });

                        _logger.LogInformation("Toll passage created successfully for exempted vehicle.");
                        return new CommandResult(Result.Success());
                    }
                }

                /// Check if the time falls within exemption time range
                if (exemptions.Any(e => e.ExemptionStartTime.HasValue && e.ExemptionEndTime.HasValue))
                {
                    foreach (var exemption in exemptions)
                    {
                        var timeOfDay = processTollRequest.Date.TimeOfDay;
                        if ((processTollRequest.Date.DayOfWeek == exemption.ExemptedDayOfWeek) && timeOfDay >= exemption.ExemptionStartTime && timeOfDay < exemption.ExemptionEndTime)
                        {
                            _logger.LogInformation("Vehicle with Id {VehicleId} is exempted from toll fee. Creating toll passage with fee 0.", vehicle.Id);

                            await _tollRepository.CreateTollPassage(new TollPassage
                            {
                                Date = processTollRequest.Date,
                                Vehicle = vehicle,
                                Fee = 0
                            });

                            _logger.LogInformation("Toll passage created successfully for exempted vehicle.");
                            return new CommandResult(Result.Success());
                        }
                    }
                }
            }

            _logger.LogInformation("Vehicle with Id {VehicleId} is not exempted from toll fee. Proceeding to charge toll fee.", vehicle.Id);
            return new CommandResult(Result.Failure("process doesn't fall within any exemptions"));
        }

        private async Task<Vehicle?> HandleVehicleData(ProcessTollRequest processTollRequest, List<ExemptedVehicleType> exemptedVehicleTypes)
        {
            _logger.LogInformation("Fetching vehicle data for vehicle with registration number: {RegistrationNumber}", processTollRequest.RegistrationNumber);
            var vehicleEntity = await _vehicleRepository.GetVehicleByRegistrationNumber(processTollRequest.RegistrationNumber);

            // If vehicleEntity is null we need to get the vehicle data from the external API
            // togheter with the owner details and vehicle type
            if (vehicleEntity == null)
            {
                _logger.LogInformation("Vehicle with registration number {RegistrationNumber} not found in the database. Fetching vehicle data from external API.", processTollRequest.RegistrationNumber);
                var vehicleData = await _vehicleAPIService.GetVehicleData(processTollRequest.RegistrationNumber);

                if (vehicleData == null)
                {
                    return null;
                }

                var vehicleType = exemptedVehicleTypes.FirstOrDefault(x => x.VehicleType == vehicleData.VehicleType);

                _logger.LogInformation("Creating vehicle with registration number {RegistrationNumber} in the database.", processTollRequest.RegistrationNumber);   
                vehicleEntity = await _vehicleRepository.CreateVehicle(new Vehicle
                {
                    RegistrationNumber = vehicleData.RegistrationNumber,
                    VehicleType = vehicleType?.VehicleType ?? VehicleType.Unknown,
                    Owner = new VehicleOwner
                    {
                        FirstName = vehicleData.VehicleOwnerResponse.FirstName,
                        LastName = vehicleData.VehicleOwnerResponse.LastName,
                        Address = vehicleData.VehicleOwnerResponse.Address
                    }
                });
            }

            return vehicleEntity;
        }
    }
}
