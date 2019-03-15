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
    public class GetBondParseHandler : IRequestHandler<GetBondParseQuery, Response<GetBondParseResponse>>
    {
        private readonly ILogger<GetBondParseHandler> _logger;
        private readonly IBondParseRepository _bondParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
        private readonly Infra.CrossCutting.Log.Logger _log;

        public GetBondParseHandler(ILogger<GetBondParseHandler> logger, AuthenticatedUser authenticatedUser, IBondParseRepository bondParseRepository)
        {
            _logger = logger;
            _bondParseRepository = bondParseRepository;
            _authenticatedUser = authenticatedUser;
            _log = new Infra.CrossCutting.Log.Logger(authenticatedUser);
        }

        public async Task<Response<GetBondParseResponse>> Handle(GetBondParseQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null)
                {
                    _logger.Warning(_log.SendLog("A requisição não pode ser nula."));
                    return Response<GetBondParseResponse>.Fail("A requisição não pode ser nula.");
                }

                _logger.Debug(_log.SendLog($"Buscando o título na base."));

                var bondParse = await _bondParseRepository.GetBy(request.BondType, request.BondIndex, request.IsAntecipatedSell);

                _logger.Debug(_log.SendLog("O título foi encontrado com sucesso."));

                return Response<GetBondParseResponse>.Ok(new GetBondParseResponse { BondParse = bondParse.ToDto() });
            }
            catch (Exception ex)
            {
                _logger.Error(_log.SendLog("Ocorreu um erro durante a busca do título na base."), ex);
                throw;
            }
        }
    }
}
