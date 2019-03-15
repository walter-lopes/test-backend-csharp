using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;

namespace Easynvest.Infohub.Parse.Application.Query.Queries
{
    public class GetBondParseQuery : IRequest<Response<GetBondParseResponse>>
    {
        public string BondType { get; set; }
        public string BondIndex { get; set; }
        public string IsAntecipatedSell { get; set; }
    }
}
