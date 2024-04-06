using GotheTollway.Domain.Interface;
using GotheTollway.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GotheTollway.Domain.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ITollService, TollService>();
        }
    }
}
