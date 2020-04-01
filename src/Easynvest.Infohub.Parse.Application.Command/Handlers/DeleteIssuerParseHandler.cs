using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;

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
       
        private readonly IMediator _mediator;

        public DeleteIssuerParseHandler(ILogger<DeleteIssuerParseHandler> logger, AuthenticatedUser authenticatedUser,IIssuerParseRepository issuerParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;

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
                    _logger.LogError("A requisição não pode ser nula.");
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                _logger.LogInformation("Buscando o registro na lista de emissores.");
                var issuerParse = await _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = request.IssuerNameCetip });

                if (issuerParse.IsFailure)
                {
                    _logger.LogError(("Ocorreu um erro durante o retorno da lista de títulos."));
                    return Response<Unit>.Fail(issuerParse.Messages);
                }

                var issuersParseFound = issuerParse.Value.IssuerParse;

                if (issuersParseFound is null)
                {
                    var messageLog = $"Não foi possível remover o parse de emissores, pois ele não existe. Emissor na Cetip: { request.IssuerNameCetip }.";

                    _logger.LogError(messageLog);
                    return Response<Unit>.Fail($"Não foi possível remover o registro. Não existe registro com nome Cetip requisitado.");
                }

                _logger.LogInformation("Iniciando a remoção do parse de emissores.");
                await _issuerParseRepository.Delete(IssuerParse.Create(issuersParseFound.IssuerNameCustodyManager, issuersParseFound.IssuerNameCetip).Value);
                _logger.LogInformation("O parser de emissores foi removido com sucesso.");

                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                 _logger.LogError("Ocorreu um erro durante a remoção do parse de emissores."+ ex);
                throw;
            }
        }
    }
}
