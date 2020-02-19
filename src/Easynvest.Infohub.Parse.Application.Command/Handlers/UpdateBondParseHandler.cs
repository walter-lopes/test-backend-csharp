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
    public class UpdateBondParseHandler : IRequestHandler<UpdateBondParseCommand, Response<Unit>>
    {
        private readonly ILogger<UpdateBondParseHandler> _logger;
        private readonly IBondParseRepository _bondParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       
        private readonly IMediator _mediator;

        public UpdateBondParseHandler(ILogger<UpdateBondParseHandler> logger, AuthenticatedUser authenticatedUser, IBondParseRepository bondParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _bondParseRepository = bondParseRepository;
            _authenticatedUser = authenticatedUser;
            
            _mediator = mediator;
        }
        public async Task<Response<Unit>> Handle(UpdateBondParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.BondParse is null)
                {
                   //("A requisição não pode ser nulla."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

                var bondRequest = request.BondParse;
                var bondParse = BondParse.Create(bondRequest.BondType, bondRequest.BondIndex, bondRequest.IsAntecipatedSell, bondRequest.IdCustodyManagerBond);

                if (bondParse.IsFailure)
                {
                    var messageLog =
                        $"Ocorreu um erro durante a criação do parse de papéis. Índice do papel: { bondRequest.BondIndex }, Tipo do papel: { bondRequest.BondType }," +
                        $"Estado do títuo: { bondRequest.IsAntecipatedSell }, Id da custódia do papel: { bondRequest.IdCustodyManagerBond }.";

                   //(messageLog));
                    return Response<Unit>.Fail(bondParse.Messages);
                }

                var bondParseValid = bondParse.Value;

                var bond = await _mediator.Send(new GetBondParseQuery
                {
                    BondType = bondParseValid.BondType,
                    BondIndex = bondParseValid.BondIndex,
                    IsAntecipatedSell = bondParseValid.IsAntecipatedSell
                });

                if (bond.IsFailure)
                {
                   //("Ocorreu um erro durante a verificação se o papel requerido já existe na base."));
                    return Response<Unit>.Fail(bond.Messages);
                }

                var bondFound = bond.Value.BondParse;
                if (bondFound is null)
                {
                    var messageLog = $"Não foi possível atualizar o parse de papel, pois o requerido não existe." +
                                     $" Tipo de papel: { bondParseValid.BondType }, Índice do papel: { bondParseValid.BondIndex }," +
                                     $" Estado de venda: { bondParseValid.IsAntecipatedSell }, Id na custódia: { bondParseValid.IdCustodyManagerBond }.";

                   //(messageLog));
                    return Response<Unit>.Fail($"Não foi possível atualizar o registro. Não existe registro com tipo IF, indexador e código resgate requisitados.");
                }

               //_log.SendLog("Iniciando a atualização do parser de papéis."));
                await _bondParseRepository.Update(bondParseValid);

               //_log.SendLog("O parser de papéis foi alterado com sucesso."));
                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                //_log.SendLog("Erro durante a atualização do parser de papéis."), ex);
                throw;
            }
        }
    }
}
