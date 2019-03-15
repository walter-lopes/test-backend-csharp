using Easynvest.Infohub.Parse.Application.Query.Dtos;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Application.Query.Responses
{
    public class GetIssuersParseResponse
    {
        public IReadOnlyCollection<IssuerParseDto> IssuersParse { get; set; }
    }
}
