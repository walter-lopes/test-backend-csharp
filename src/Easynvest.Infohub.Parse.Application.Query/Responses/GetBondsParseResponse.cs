using Easynvest.Infohub.Parse.Application.Query.Dtos;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Application.Query.Responses
{
    public class GetBondsParseResponse
    {
        public IReadOnlyCollection<BondParseDto> BondsParse { get; set; }
    }
}
