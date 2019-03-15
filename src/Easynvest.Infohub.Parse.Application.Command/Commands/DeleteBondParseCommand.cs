using Easynvest.Infohub.Parse.Application.Command.Dtos;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;

namespace Easynvest.Infohub.Parse.Application.Command.Commands
{
    public class DeleteBondParseCommand : IRequest<Response<Unit>>
    {
        public string BondType { get; set; }
        public string BondIndex { get; set; }
        public string IsAntecipatedSell { get; set; }
    }
}
