using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class ConfigureOptionsContainerEx
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CorsOptions>(option => configuration.GetSection("CorsOptions").Bind(option));
            services.Configure<ConnectionStringOption>(connectionStringOptions =>
            {
                connectionStringOptions.OracleConnection = configuration.GetConnectionString("OracleConnection");
            });

            return services;
        }
    }
}
