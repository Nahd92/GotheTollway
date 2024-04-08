using GotheTollway.Domain.Configuration;
using GotheTollway.Domain.Interface;
using GotheTollway.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GotheTollway.Infrastructure.Configurations
{
    public static class ServiceCollectionExtension
    {   
        public static void ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleAPIService, VehicleAPIService>();
        }
    }
}
