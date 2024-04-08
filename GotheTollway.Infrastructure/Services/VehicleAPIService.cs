using GotheTollway.Domain.Enums;
using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Models.VehicleAPI;
using GotheTollway.Infrastructure.MoqData;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GotheTollway.Infrastructure.Services
{
    /// <summary>
    /// This is just a mock class to simulate a Vehicle API
    /// And how mock data can be returned with a real API
    /// Through HttpClient and HttpClientFactory
    /// </summary>
    public class VehicleAPIService(ILogger<VehicleAPIService> logger) : IVehicleAPIService
    {
        public async Task<VehicleResponse?> GetVehicleData(string registrationNumber)
        {
            try
            {
                var response = await HttpClientMock.HttpClient(registrationNumber).GetAsync($"vehicledata/{registrationNumber}");

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Error while fetching vehicle data");
                    throw new Exception("Error while fetching vehicle data");
                }

                return response.Content.ReadFromJsonAsync<VehicleResponse>().Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while fetching vehicle data.");
                return new VehicleResponse();
            }
        }
    }
}
