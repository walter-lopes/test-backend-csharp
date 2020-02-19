using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache;
using Easynvest.Infohub.Parse.Infra.Data.HealthChecks;
using Easynvest.Infohub.Parse.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using Easynvest.Infohub.Parse.Infra.Data.Redis;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class RepositoriesContainerEx
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IssuerParseCacheRepository>()
                .AddTransient<IssuerParseRepository>()
                .AddTransient<IBondParseRepository, BondParseRepository>()
                .AddTransient<ICache, RedisCache>()
                .AddTransient<Func<RepositoryType, IIssuerParseRepository>>(serviceProvider => key =>
                {
                    switch (key)
                    {
                        case RepositoryType.Cache:
                            return serviceProvider.GetService<IssuerParseCacheRepository>();
                        default:
                            return serviceProvider.GetService<IssuerParseRepository>();
                    }
                })
              
                .AddTransient<IHealthCheckRepository, DatabaseHealthCheckRepository>();
        }
    }
}
