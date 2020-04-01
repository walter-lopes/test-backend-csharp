using Easynvest.Infohub.Parse.Application.Query.Adapters;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Application.Query.Handlers
{
    public class GetIssuersParseHandler : IRequestHandler<GetIssuersParseQuery, Response<GetIssuersParseResponse>>
    {
        private readonly ILogger<GetIssuersParseHandler> _logger;
        private readonly IIssuerParseRepository _issuerParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       

        public GetIssuersParseHandler(ILogger<GetIssuersParseHandler> logger, AuthenticatedUser authenticatedUser, IIssuerParseRepository issuerParseRepository)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _issuerParseRepository = issuerParseRepository;
        }

        public async Task<Response<GetIssuersParseResponse>> Handle(GetIssuersParseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Iniciando o retorno de parse de emissores.");

                var issuersParse = await _issuerParseRepository.GetAll();

                return Response<GetIssuersParseResponse>.Ok(new GetIssuersParseResponse { IssuersParse = issuersParse.ToDto() });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro durante o retorno do parse de emissores."+ ex);
                throw;
            }
        }
    }
}
