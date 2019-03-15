using Easynvest.Infohub.Parse.Infra.CrossCutting.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class CorsExtensions
    {
        public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IHostingEnvironment env) => app.UseCors(GetPolicy(env));

        private static string GetPolicy(IHostingEnvironment env) => env.EnvironmentName == "Homolog" ? "AllowAll" : "ByDomain";

        public static IServiceCollection ConfigureCorsService(this IServiceCollection service)
        {
            var provider = service.BuildServiceProvider();
            var corsOptions = provider.GetService<IOptions<CorsOptions>>().Value;
            var domainsAllowed = corsOptions.DomainsAllowed;

            return service.AddCors(options =>
            {
                options
                    .AddPolicy("AllowAll",
                        builder =>
                        {
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                options
                    .AddPolicy("ByDomain",
                        builder =>
                        {
                            builder
                                .WithOrigins(domainsAllowed)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        }
                    );
            });
        }
    }
}
