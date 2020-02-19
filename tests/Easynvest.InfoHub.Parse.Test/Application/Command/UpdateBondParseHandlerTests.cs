using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;
using Easynvest.Infohub.Parse.Application.Command.Dtos;

namespace Easynvest.InfoHub.Parse.Test.Application.Command
{
    public class UpdateBondParseHandlerTests
    {
        private ILogger<UpdateBondParseHandler> _logger;
        private UpdateBondParseHandler _updateBondHandler;
        private IBondParseRepository _bondParseRepository;
        private AuthenticatedUser _authenticatedUser;
       
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<UpdateBondParseHandler>>();
            _bondParseRepository = Substitute.For<IBondParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _mediator = Substitute.For<IMediator>();
            _updateBondHandler = new UpdateBondParseHandler(_logger, _authenticatedUser, _bondParseRepository, _mediator);
        }

        [Test]
        [TestCase("DI", "CDI", "CDI", 0)]
        public void Should_Throws_Exception_When_Update_Bond(string bondType, string bondIndex, string isAntecipatedSell, int idCustodyManagerBond)
        {
            var bondParse = new BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell,
                IdCustodyManagerBond = idCustodyManagerBond
            };
            var bondParseFound = new Infohub.Parse.Application.Query.Dtos.BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell,
                IdCustodyManagerBond = idCustodyManagerBond
            };
            var bondsResponse = new GetBondParseResponse { BondParse = bondParseFound };

            _mediator.Send(new GetBondParseQuery { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell })
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            _mediator.Send(new GetBondParseQuery()).ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var updateBondRequest = new UpdateBondParseCommand { BondParse = bondParse };
            _bondParseRepository.Update(Arg.Any<BondParse>()).Returns(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _updateBondHandler.Handle(updateBondRequest, CancellationToken.None));
        }

        [Test]
        [TestCase("DI", "DI", null, 10)]
        [TestCase("DI", null, "DI", 10)]
        [TestCase(null, "DI", "DI", 10)]
        [TestCase(null, null, null, 10)]
        [TestCase("", "DI", null, 10)]
        [TestCase("", "DI", "DI", 10)]
        public void Should_Return_Failure_When_Update_With_Invalid_Parameters(string bondType, string bondIndex, string isAntecipatedSell, int idCustodyManagerBond)
        {
            var bondParse = new BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell,
                IdCustodyManagerBond = idCustodyManagerBond
            };
            var updateBondRequest = new UpdateBondParseCommand {BondParse = bondParse};
            var response = _updateBondHandler.Handle(updateBondRequest, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response.Result);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
            });
        }

        [Test]
        public void Should_Return_Failure_When_Command_Is_Null()
        {
            var response = _updateBondHandler.Handle(null, CancellationToken.None);

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
