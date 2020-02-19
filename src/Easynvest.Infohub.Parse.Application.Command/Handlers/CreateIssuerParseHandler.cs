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
    public class CreateIssuerParseHandler : IRequestHandler<CreateIssuerParseCommand, Response<Unit>>
    {
        private readonly ILogger<CreateIssuerParseHandler> _logger;
        private readonly IIssuerParseRepository _issuerParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       
        private readonly IMediator _mediator;

        public CreateIssuerParseHandler(ILogger<CreateIssuerParseHandler> logger, AuthenticatedUser authenticatedUser, Func<RepositoryType, IIssuerParseRepository> issuerParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            
            _issuerParseRepository = issuerParseRepository(RepositoryType.Cache);
            _mediator = mediator;
        }

        public async Task<Response<Unit>> Handle(CreateIssuerParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.IssuerParse is null)
                {
                   //("A requisição não pode ser nula."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                var issuerParse = IssuerParse.Create(request.IssuerParse.IssuerNameCustodyManager, request.IssuerParse.IssuerNameCetip);

                if (issuerParse.IsFailure)
                {
                    var messageLog =
                        $"Ocorreu um erro durante a criação do parse de emissores. Nome do emissor na Cetip: {request.IssuerParse.IssuerNameCetip}, " +
                        $"Nome do emissor na custódia: {request.IssuerParse.IssuerNameCustodyManager}.";

                   //(messageLog));
                    return Response<Unit>.Fail(issuerParse.Messages);
                }

                var issuerParseValid = issuerParse.Value;

                var issuer = await _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerParseValid.IssuerNameCetip });
                var issuerFound = issuer.Value.IssuerParse;

                if (issuer.IsFailure)
                {
                   //($"Ocorreu um erro durante a verificação de registro do emissor. {issuerParse.Value.IssuerNameCetip}"));
                    return Response<Unit>.Fail(issuer.Messages);
                }

                if (issuerFound != null)
                {
                    var messageLog = $"Não foi possível criar o parse de emissores, pois o requerido já é um parse de emissores existente." +
                                     $" Emissor na Cetip: {issuerParseValid.IssuerNameCetip}, Emissor na custódia: { issuerParseValid.IssuerNameCustodyManager}.";

                   //(messageLog));
                    return Response<Unit>.Fail($"Não foi possível criar o registro. Já existe um registro com nome do emissor requisitado.");
                }

               //_log.SendLog("Iniciando a criação do parse de emissores."));
                await _issuerParseRepository.Create(issuerParseValid);
               //_log.SendLog("O parse de emissores foi criado com sucesso."));

                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                //_log.SendLog("Ocorreu um erro durante a criação do parse de emissores."), ex);
                throw;
            }
        }
    }
}
