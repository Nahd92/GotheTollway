﻿using GotheToll.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GotheTollway.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GotheTollway.Database.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GotheTollWayContext>((provider, options) =>
            {
                var settings = provider.GetRequiredService<IOptions<InfrastructureSettings>>().Value;
                options.UseSqlServer(settings.GotheTollwayDBConnectionString,
                                     o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            }).AddSingleton<InfrastructureSettings>();
        }
    }
}