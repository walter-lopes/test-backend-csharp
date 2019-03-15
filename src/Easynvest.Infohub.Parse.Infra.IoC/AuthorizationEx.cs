using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class AuthorizationEx
    {
        public static IServiceCollection AddAuthenticatedUser(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<AuthenticatedUser>();

            return services;
        }
    }
}
