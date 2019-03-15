using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using Easynvest.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Application.Command.Handlers
{
    public class DeleteIssuerParseHandler : IRequestHandler<DeleteIssuerParseCommand, Response<Unit>>
    {
        private readonly ILogger<DeleteIssuerParseHandler> _logger;
        private readonly IIssuerParseRepository _issuerParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
        private readonly Infra.CrossCutting.Log.Logger _log;
        private readonly IMediator _mediator;

        public DeleteIssuerParseHandler(ILogger<DeleteIssuerParseHandler> logger, AuthenticatedUser authenticatedUser, IIssuerParseRepository issuerParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _log = new Infra.CrossCutting.Log.Logger(_authenticatedUser);
            _issuerParseRepository = issuerParseRepository;
            _mediator = mediator;
        }

        public AuthenticatedUser AuthenticatedUser => _authenticatedUser;

        public async Task<Response<Unit>> Handle(DeleteIssuerParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.IssuerNameCetip is null)
                {
                    _logger.Warning(_log.SendLog("A requisição não pode ser nula."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                _logger.Warning(_log.SendLog("Buscando o registro na lista de emissores."));
                var issuerParse = await _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = request.IssuerNameCetip });

                if (issuerParse.IsFailure)
                {
                    _logger.Warning(_log.SendLog("Ocorreu um erro durante o retorno da lista de títulos."));
                    return Response<Unit>.Fail(issuerParse.Messages);
                }

                var issuersParseFound = issuerParse.Value.IssuerParse;

                if (issuersParseFound is null)
                {
                    var messageLog = $"Não foi possível remover o parse de emissores, pois ele não existe. Emissor na Cetip: { request.IssuerNameCetip }.";

                    _logger.Warning(_log.SendLog(messageLog));
                    return Response<Unit>.Fail($"Não foi possível remover o registro. Não existe registro com nome Cetip requisitado.");
                }

                _logger.Debug(_log.SendLog("Iniciando a remoção do parse de emissores."));
                await _issuerParseRepository.Delete(IssuerParse.Create(issuersParseFound.IssuerNameCustodyManager, issuersParseFound.IssuerNameCetip).Value);
                _logger.Debug(_log.SendLog("O parser de emissores foi removido com sucesso."));

                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                _logger.Error(_log.SendLog("Ocorreu um erro durante a remoção do parse de emissores."), ex);
                throw;
            }
        }
    }
}
