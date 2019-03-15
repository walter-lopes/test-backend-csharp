using Easynvest.Infohub.Parse.Application.Command.Dtos;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;

namespace Easynvest.Infohub.Parse.Application.Command.Commands
{
    public class CreateIssuerParseCommand : IRequest<Response<Unit>>
    {
        public IssuerParseDto IssuerParse { get; set; }
    }
}
