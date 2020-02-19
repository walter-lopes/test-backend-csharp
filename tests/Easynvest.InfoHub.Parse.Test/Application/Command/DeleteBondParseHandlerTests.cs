using System;
using System.Threading;
using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Dtos;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Application.Command
{
    public class DeleteBondParseHandlerTests
    {
        private ILogger<DeleteBondParseHandler> _logger;
        private DeleteBondParseHandler _deleteBondHandler;
        private IBondParseRepository _bondParseRepository;
        private AuthenticatedUser _authenticatedUser;
       
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<DeleteBondParseHandler>>();
            _bondParseRepository = Substitute.For<IBondParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _mediator = Substitute.For<IMediator>();
            _deleteBondHandler = new DeleteBondParseHandler(_logger, _authenticatedUser, _bondParseRepository, _mediator);
        }

        [Test]
        [TestCase("AAA", "AA", "AA", 10)]
        [TestCase("AAA", "-", "AA", 10)]
        public void Should_Return_Success_When_Delete(string bondType, string bondIndex, string isAntecipatedSell, decimal idCustodyManagerBond)
        {
            var bondParseFound = new BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IdCustodyManagerBond = idCustodyManagerBond,
                IsAntecipatedSell = isAntecipatedSell
            };
            var bondsResponse = new GetBondParseResponse { BondParse = bondParseFound };

            _mediator.Send(new GetBondParseQuery { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell })
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            _mediator.Send(new GetBondParseQuery()).ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var deleteBondParseCommand = new DeleteBondParseCommand {BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell};
            var response = _deleteBondHandler.Handle(deleteBondParseCommand, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }

        [Test]
        [TestCase("AAA", "AA", "AA", 10)]
        [TestCase("AAA", "-", "AA", 10)]
        [TestCase("-", "-", "-", 10)]
        [TestCase("-", "AA", "-", 10)]
        public void Should_Throw_Exception_When_Delete(string bondType, string bondIndex, string isAntecipatedSell, decimal idCustodyManagerBond)
        {
            var bondParseFound = new BondParseDto
            {
                BondIndex = bondIndex,
                BondType = bondType,
                IdCustodyManagerBond = idCustodyManagerBond,
                IsAntecipatedSell = isAntecipatedSell
            };
            var bondsResponse = new GetBondParseResponse { BondParse = bondParseFound };

            _mediator.Send(new GetBondParseQuery { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell })
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            _mediator.Send(new GetBondParseQuery()).ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var deleteBondParseCommand = new DeleteBondParseCommand { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell };

            _bondParseRepository.Delete(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _deleteBondHandler.Handle(deleteBondParseCommand, CancellationToken.None));
        }

        [Test]
        public void Should_Throw_Failure_When_Parameter_Is_Null()
        {
            var request = new DeleteBondParseCommand();
            var response = _deleteBondHandler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
            });
        }

        [Test]
        public void Should_Return_Failure_When_Command_Is_Null()
        {
            var response = _deleteBondHandler.Handle(null, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
                Assert.IsTrue(response.Result.Messages.Contains("A requisição não pode ser nula."));
            });
        }
    }
}
