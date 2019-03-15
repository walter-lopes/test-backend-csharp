using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;

namespace Easynvest.Infohub.Parse.Application.Query.Queries
{
    public class GetIssuersParseQuery : IRequest<Response<GetIssuersParseResponse>>
    {
    }
}
