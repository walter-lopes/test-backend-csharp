using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.Data.HealthChecks;
using Easynvest.Infohub.Parse.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class RepositoriesContainerEx
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IBondParseRepository, BondParseRepository>()
                .AddTransient<IIssuerParseRepository, IssuerParseRepository>()
                .AddTransient<IHealthCheckRepository, DatabaseHealthCheckRepository>();
        }
    }
}
