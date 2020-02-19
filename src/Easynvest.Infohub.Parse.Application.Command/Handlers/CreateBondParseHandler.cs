using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;

using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Easynvest.Infohub.Parse.Application.Command.Handlers
{
    public class CreateBondParseHandler : IRequestHandler<CreateBondParseCommand, Response<Unit>>
    {
        private readonly ILogger<CreateBondParseHandler> _logger;
        private readonly IBondParseRepository _bondParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       
        private readonly IMediator _mediator;

        public CreateBondParseHandler(ILogger<CreateBondParseHandler> logger, AuthenticatedUser authenticatedUser, IBondParseRepository bondParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _bondParseRepository = bondParseRepository;
            _authenticatedUser = authenticatedUser;
            
            _mediator = mediator;
        }

        public async Task<Response<Unit>> Handle(CreateBondParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.BondParse is null)
                {
                   //("A requisição não pode ser nula."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                var bond = BondParse.Create(request.BondParse.BondType, request.BondParse.BondIndex, request.BondParse.IsAntecipatedSell,
                    request.BondParse.IdCustodyManagerBond);

                if (bond.IsFailure)
                {
                    var messageLog =
                        $"Ocorreu um erro durante a criação do parse de papéis. Índice do papel: {request.BondParse.BondIndex}, Tipo do papel: {request.BondParse.BondType}," +
                        $"Estado do títuo: {request.BondParse.IsAntecipatedSell}, Id da custódia do papel: {request.BondParse.IdCustodyManagerBond}.";

                   //(messageLog));
                    return Response<Unit>.Fail(bond.Messages);
                }

                var bondParseValid = bond.Value;

                var bondParseFound = await _mediator.Send(new GetBondParseQuery
                {
                    BondType = bondParseValid.BondType,
                    BondIndex = bondParseValid.BondIndex,
                    IsAntecipatedSell = bondParseValid.IsAntecipatedSell
                });

                if (bondParseFound.IsFailure)
                {
                   //("Ocorreu um erro durante o retorno do índice."));
                    return Response<Unit>.Fail(bondParseFound.Messages);
                }

                if (bondParseFound.Value.BondParse != null)
                {
                    var messageLog =
                        $"Não foi possível criar o parse de papel, pois o requerido já é um parse de papel existente." +
                        $" Tipo de papel: { bondParseValid.BondType }, Índice do papel: { bondParseValid.BondIndex }," +
                        $" Estado de venda: { bondParseValid.IsAntecipatedSell }, Id na custódia: { bondParseValid.IdCustodyManagerBond }.";

                   //(messageLog));
                    return Response<Unit>.Fail($"Não foi possível criar o registro. Já existe um registro com o tipo, " +
                                                                  $"índice e estado da venda do papel requisitado.");
                }

               //_log.SendLog("Iniciando a criação do parse dos papéis"));
                await _bondParseRepository.Create(bondParseValid);
               //_log.SendLog("O parse dos papéis foram inseridos com sucesso."));

                return Response<Unit>.Ok();
            }
            catch (Exception ex)
            {
                //_log.SendLog("Erro durante a criação do parser de papéis."), ex);
                throw;
            }
        }
    }
}
