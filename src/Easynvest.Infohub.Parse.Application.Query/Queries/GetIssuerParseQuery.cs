using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;

namespace Easynvest.Infohub.Parse.Application.Query.Queries
{
    public class GetIssuerParseQuery : IRequest<Response<GetIssuerParseResponse>>
    {
        public string IssuerNameCetip { get; set; }
    }
}
