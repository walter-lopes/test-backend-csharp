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
    public class GetIssuerParseHandler : IRequestHandler<GetIssuerParseQuery, Response<GetIssuerParseResponse>>
    {
        private readonly ILogger<GetIssuerParseHandler> _logger;
        private readonly IIssuerParseRepository _issuerParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       

        public GetIssuerParseHandler(ILogger<GetIssuerParseHandler> logger, AuthenticatedUser authenticatedUser, Func<RepositoryType, IIssuerParseRepository> issuerParseRepository)
        {
            _logger = logger;
            _issuerParseRepository = issuerParseRepository(RepositoryType.Cache);
            _authenticatedUser = authenticatedUser;
            
        }
        public async Task<Response<GetIssuerParseResponse>> Handle(GetIssuerParseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.IssuerNameCetip is null)
                {
                   //("A requisição não pode ser nula."));
                    return Response<GetIssuerParseResponse>.Fail("A requisição não pode ser nula.");
                }

               //_log.SendLog("Buscando o emissor na base."));

                var issuerParse = await _issuerParseRepository.GetBy(request.IssuerNameCetip);

               //_log.SendLog("O emissor foi encontrado com sucesso."));

                return Response<GetIssuerParseResponse>.Ok(new GetIssuerParseResponse { IssuerParse = issuerParse.ToDto() });
            }
            catch (Exception ex)
            {
                //_log.SendLog("Ocorreu um erro durante a busca do emissor na base."), ex);
                throw;
            }
        }
    }
}
