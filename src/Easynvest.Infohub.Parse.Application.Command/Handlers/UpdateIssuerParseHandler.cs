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
    public class UpdateIssuerParseHandler : IRequestHandler<UpdateIssuerParseCommand, Response<Unit>>
    {
        private readonly ILogger<UpdateIssuerParseHandler> _logger;
        private readonly IIssuerParseRepository _issuerParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
        private readonly Infra.CrossCutting.Log.Logger _log;
        private readonly IMediator _mediator;

        public UpdateIssuerParseHandler(ILogger<UpdateIssuerParseHandler> logger, AuthenticatedUser authenticatedUser, IIssuerParseRepository issuerParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _log = new Infra.CrossCutting.Log.Logger(_authenticatedUser);
            _issuerParseRepository = issuerParseRepository;
            _mediator = mediator;
        }

        public async Task<Response<Unit>> Handle(UpdateIssuerParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.IssuerParse is null)
                {
                    _logger.Warning(_log.SendLog("A requisição não pode ser nula."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                var issuerRequest = request.IssuerParse;
                var issuerParse = IssuerParse.Create(issuerRequest.IssuerNameCustodyManager, issuerRequest.IssuerNameCetip);

                if (issuerParse.IsFailure)
                {
                    var messageLog =
                        $"Ocorreu um erro durante a atualização do parse de emissores. Nome do emissor na Cetip: {issuerRequest.IssuerNameCetip}, " +
                        $"Nome do emissor na custódia: {issuerRequest.IssuerNameCustodyManager}.";

                    _logger.Warning(_log.SendLog(messageLog));
                    return Response<Unit>.Fail(issuerParse.Messages);
                }

                var issuerParseValid = issuerParse.Value;
                var issuer = await _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerParseValid.IssuerNameCetip });
                var issuerFound = issuer.Value.IssuerParse;

                if (issuerParse.IsFailure)
                {
                    _logger.Warning(_log.SendLog($"Ocorreu um erro durante a verificação de registro do título. { issuerParseValid.IssuerNameCetip }"));
                    return Response<Unit>.Fail(issuerParse.Messages);
                }

                if (issuerFound is null)
                {
                    var messageLog = $"Não foi possível atualizar o parse de emissores, pois o requerido não existe." +
                                     $" Emissor na Cetip: { issuerParseValid.IssuerNameCetip }, Emissor na custódia: { issuerParseValid.IssuerNameCustodyManager }.";

                    _logger.Warning(_log.SendLog(messageLog));
                    return Response<Unit>.Fail($"Não foi possível atualizar o registro. Não existe resgistro com nome do emissor da Cetip requisitado.");
                }

                _logger.Debug(_log.SendLog("Iniciando a atualização do parse de emissores."));
                await _issuerParseRepository.Update(issuerParse.Value);
                _logger.Debug(_log.SendLog("O parse de emissores foi atualizado com sucesso."));

                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                _logger.Error(_log.SendLog("Ocorreu um erro durante a atualização do parse de emissores."), ex);
                throw;
            }
        }
    }
}
