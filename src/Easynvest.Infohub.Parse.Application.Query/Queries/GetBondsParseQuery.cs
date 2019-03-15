using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easynvest.Infohub.Parse.Application.Query.Queries
{
    public class GetBondsParseQuery : IRequest<Response<GetBondsParseResponse>>
    {
    }
}
