using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache;
using Easynvest.Infohub.Parse.Infra.Data.HealthChecks;
using Easynvest.Infohub.Parse.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Easynvest.Infohub.Parse.Infra.Data.Redis;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class RepositoriesContainerEx
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IBondParseRepository, BondParseRepository>()
                .AddScoped<ICache, RedisCache>()
                .AddTransient<IIssuerParseRepository, IssuerParseRepository>()
                .Decorate<IIssuerParseRepository, IssuerParseCacheRepository>()
                .AddTransient<IHealthCheckRepository, DatabaseHealthCheckRepository>();
        }
    }
}
