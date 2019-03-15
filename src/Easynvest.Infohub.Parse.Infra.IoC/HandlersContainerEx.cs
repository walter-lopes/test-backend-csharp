using Easynvest.Infohub.Parse.Application.Command.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Easynvest.Infohub.Parse.Infra.IoC
{
    public static class HandlersContainerEx
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(CreateBondParseHandler).Assembly)
                .AddMediatR(typeof(CreateIssuerParseHandler).Assembly)
                .AddMediatR(typeof(DeleteBondParseHandler).Assembly)
                .AddMediatR(typeof(DeleteIssuerParseHandler).Assembly)
                .AddMediatR(typeof(UpdateBondParseHandler).Assembly)
                .AddMediatR(typeof(UpdateIssuerParseHandler).Assembly)
                .AddMediatR(typeof(GetBondParseHandler).Assembly)
                .AddMediatR(typeof(GetBondsParseHandler).Assembly)
                .AddMediatR(typeof(GetIssuerParseHandler).Assembly)
                .AddMediatR(typeof(GetIssuersParseHandler).Assembly);
        }
    }
}
