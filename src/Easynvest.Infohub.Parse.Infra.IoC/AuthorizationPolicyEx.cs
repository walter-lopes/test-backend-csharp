using Microsoft.Extensions.DependencyInjection;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class AuthorizationPolicyEx
    {
        public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiresInfoHubParseParseIndexReadWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_INDEX_READ",
                        "GRP_EASYNVEST_API_TICKETMANAGER_PARSE_INDEX_WRITE"));

                options.AddPolicy("RequiresInfoHubParseIndexWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_INDEX_WRITE"));

                options.AddPolicy("RequiresInfoHubParseBondReadWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_BOND_READ",
                        "GRP_EASYNVEST_API_TICKETMANAGER_PARSE_BOND_WRITE"));

                options.AddPolicy("RequiresInfoHubParseBondWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_BOND_WRITE"));

                options.AddPolicy("RequiresInfoHubParseIssuerReadWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_ISSUER_READ",
                        "GRP_EASYNVEST_API_TICKETMANAGER_PARSE_ISSUER_WRITE"));

                options.AddPolicy("RequiresInfoHubParseIssuerWriteRole",
                    policy => policy.RequireRole("GRP_EASYNVEST_API_TICKETMANAGER_PARSE_ISSUER_WRITE"));
            });

            return services;
        }
    }
}
