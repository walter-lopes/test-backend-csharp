using Easynvest.Infohub.Parse.Application.Query.Adapters;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using Easynvest.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Application.Query.Handlers
{
    public class GetBondsParseHandler : IRequestHandler<GetBondsParseQuery, Response<GetBondsParseResponse>>
    {
        private readonly ILogger<GetBondsParseHandler> _logger;
        private readonly IBondParseRepository _bondParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
        private readonly Infra.CrossCutting.Log.Logger _log;

        public GetBondsParseHandler(ILogger<GetBondsParseHandler> logger, AuthenticatedUser authenticatedUser, IBondParseRepository bondParseRepository)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _bondParseRepository = bondParseRepository;
            _log = new Infra.CrossCutting.Log.Logger(authenticatedUser);
        }

        public async Task<Response<GetBondsParseResponse>> Handle(GetBondsParseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Debug(_log.SendLog("Iniciando o retorno da lista de títulos."));

                var bondsParse = await _bondParseRepository.GetAll();

                _logger.Debug(_log.SendLog("Lista de parse de títulos retornada com sucesso."));
                return Response<GetBondsParseResponse>.Ok(new GetBondsParseResponse { BondsParse = bondsParse.ToDto() });
            }
            catch (Exception ex)
            {
                _logger.Error(_log.SendLog("Ocorreu um erro durante o retorno da lista de títulos."), ex);
                throw;
            }
        }
    }
}
