using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Query.Queries;
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
    public class DeleteBondParseHandler : IRequestHandler<DeleteBondParseCommand, Response<Unit>>
    {
        private readonly ILogger<DeleteBondParseHandler> _logger;
        private readonly IBondParseRepository _bondParseRepository;
        private readonly AuthenticatedUser _authenticatedUser;
       
        private readonly IMediator _mediator;
        public DeleteBondParseHandler(ILogger<DeleteBondParseHandler> logger, AuthenticatedUser authenticatedUser, IBondParseRepository bondParseRepository,
            IMediator mediator)
        {
            _logger = logger;
            _bondParseRepository = bondParseRepository;
            _authenticatedUser = authenticatedUser;
            
            _mediator = mediator;
        }
        public async Task<Response<Unit>> Handle(DeleteBondParseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request is null || request.BondIndex is null || request.BondType is null || request.IsAntecipatedSell is null)
                {
                   //("A requisição não pode ser nula."));
                    return Response<Unit>.Fail("A requisição não pode ser nula.");
                }

               //("Buscando o registro entre a lista de papéis."));
                var bondParse = await _mediator.Send(new GetBondParseQuery
                {
                    BondType = request.BondType,
                    BondIndex = request.BondIndex,
                    IsAntecipatedSell = request.IsAntecipatedSell
                });

                if (bondParse.IsFailure)
                {
                   //("Ocorreu um erro durante o retorno da lista de papéis."));
                    return Response<Unit>.Fail(bondParse.Messages);
                }

                var bondsParseFound = bondParse.Value.BondParse;

                if (bondsParseFound is null)
                {
                    var messageLog = $"Não foi possível remover o parse de papel, pois ele não existe." +
                                     $" Tipo de papel: { request.BondType }, Índice do papel: { request.BondIndex }," +
                                     $" Estado de venda: { request.IsAntecipatedSell }.";

                   //(messageLog));
                    return Response<Unit>.Fail($"Não foi possível remover o registro. Não existe registro com tipo IF, indexador e código resgate requisitados.");
                }

               //_log.SendLog("Iniciando a remoção do parse de papéis."));
                await _bondParseRepository.Delete(request.BondIndex, request.BondType, request.IsAntecipatedSell);
               //_log.SendLog("O parser de papéis foi removido com sucesso."));

                return Response<Unit>.Ok(new Unit());
            }
            catch (Exception ex)
            {
                //_log.SendLog("Erro durante a remoção do parse de papéis."), ex);
                throw;
            }
        }
    }
}
