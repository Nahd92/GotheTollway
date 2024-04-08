using Moq.Protected;
using Moq;
using GotheTollway.Domain.Models.VehicleAPI;
using GotheTollway.Domain.Enums;
using System.Net;
using System.Text.Json;

namespace GotheTollway.Infrastructure.MoqData
{
    public static class HttpClientMock
    {
        public static HttpClient HttpClient(string registrationNumber)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(() =>
                    {
                        // Generate random data
                        Random random = new Random();
                        string randomFirstName = GenerateRandomSwedishFirstName();
                        string randomLastName = GenerateRandomSwedishLastName();
                        string randomAddress = GenerateRandomSwedishAddress();

                        var responseContent = new VehicleResponse
                        {
                            RegistrationNumber = registrationNumber,
                            VehicleOwnerResponse = new VehicleOwnerResponse
                            {
                                FirstName = randomFirstName,
                                LastName = randomLastName,
                                Address = randomAddress
                            },
                            VehicleType = (VehicleType)random.Next(Enum.GetValues(typeof(VehicleType)).Length)
                        };

                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(responseContent))
                        };
                    });

            return new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.transportstyrelsen.com/"),
            };
        }

        static string GenerateRandomSwedishFirstName()
        {
            string[] swedishFirstNames = { "Elsa", "Karl", "Sara", "Anders", "Emma", "Gustav", "Hanna", "Per", "Johanna", "Lars" };
            Random random = new Random();
            return swedishFirstNames[random.Next(swedishFirstNames.Length)];
        }

        static string GenerateRandomSwedishLastName()
        {
            string[] swedishLastNames = { "Andersson", "Johansson", "Karlsson", "Nilsson", "Eriksson", "Larsson", "Olsson", "Persson", "Svensson", "Gustafsson" };
            Random random = new Random();
            return swedishLastNames[random.Next(swedishLastNames.Length)];
        }
        static string GenerateRandomSwedishAddress()
        {
            string[] swedishStreets = { "Götgatan", "Sveavägen", "Kungsgatan", "Drottninggatan", "Storgatan", "Linnégatan", "Vasagatan", "Folkungagatan", "Karlavägen", "Södra vägen" };
            string[] swedishCities = { "Stockholm", "Göteborg", "Malmö", "Uppsala", "Linköping", "Örebro", "Västerås", "Helsingborg", "Jönköping", "Norrköping" };

            Random random = new Random();
            string street = swedishStreets[random.Next(swedishStreets.Length)];
            string city = swedishCities[random.Next(swedishCities.Length)];

            return $"{random.Next(1, 100)} {street}, {city}";
        }
    }
}
