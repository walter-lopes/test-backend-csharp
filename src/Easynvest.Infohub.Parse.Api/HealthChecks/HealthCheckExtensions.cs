using Easynvest.Infohub.Parse.Infra.Data.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using App.Metrics.Health;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;

namespace Easynvest.Infohub.Parse.Api.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services.AddHealth(builder =>
            {
                var provider = services.BuildServiceProvider();

                var configureHttp = provider.GetService<IOptionsSnapshot<ConfigureHttpHealthCheckOption>>().Value;
                var databaseHealthCheckRepository = provider.GetService<IHealthCheckRepository>();

                builder.OutputHealth.AsJson();
                builder.HealthChecks.AddCheck(new DatabaseHealthCheck(databaseHealthCheckRepository));

            });

            return services;
        }
    }
}
