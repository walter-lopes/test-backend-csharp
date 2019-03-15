using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Dtos;
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

namespace Easynvest.InfoHub.Parse.Test.Application.Command
{
    public class CreateBondParseHandlerTests
    {
        private ILogger<CreateBondParseHandler> _logger;
        private CreateBondParseHandler _createHandler;
        private IBondParseRepository _bondParseRepository;
        private AuthenticatedUser _authenticatedUser;
        private Infohub.Parse.Infra.CrossCutting.Log.Logger _log;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<CreateBondParseHandler>>();
            _bondParseRepository = Substitute.For<IBondParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            _log = new Infohub.Parse.Infra.CrossCutting.Log.Logger(_authenticatedUser);
            _mediator = Substitute.For<IMediator>();
            _createHandler = new CreateBondParseHandler(_logger, _authenticatedUser, _bondParseRepository, _mediator);
        }

        [Test]
        [TestCase("AAA", "BBB", "CCC", 10)]
        [TestCase("AAA", "-", "-", 0)]
        public void Return_Success_With_Without_Exception(string bondType, string bondIndex, string isAntecipatedSell,
            int idCustodyManagerBond)
        {
            var bondsResponse = new GetBondParseResponse();
            var bondParse = new BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell,
                IdCustodyManagerBond = idCustodyManagerBond
            };

            _mediator.Send(new GetBondParseQuery
                    {BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell})
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var request = new BondParseDto();

            var createIndexRequest = new CreateBondParseCommand {BondParse = bondParse};
            var response = _createHandler.Handle(createIndexRequest, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }

        [Test]
        [TestCase("AAA", "BBB", "CCC", 10)]
        [TestCase("AAA", "-", "-", 0)]
        public void Return_Throws_Exception_When_Create(string bondType, string bondIndex, string isAntecipatedSell,
            int idCustodyManagerBond)
        {
            var bondsResponse = new GetBondParseResponse();
            var bondParse = new BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell,
                IdCustodyManagerBond = idCustodyManagerBond
            };

            _mediator.Send(new GetBondParseQuery
                    {BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell})
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var createBondRequest = new CreateBondParseCommand {BondParse = bondParse};

            _bondParseRepository.Create(Arg.Any<BondParse>()).Returns(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () =>
                await _createHandler.Handle(createBondRequest, CancellationToken.None));
        }

        [Test]
        [TestCase("AAA", "AAA", null, 0)]
        [TestCase("BBB", null, "BBB", 0)]
        [TestCase(null, "CCC", "CCC", 0)]
        [TestCase("", "CCC", "CCC", 0)]
        [TestCase("", "", "", 0)]
        [TestCase(null, null, null, 0)]
        [TestCase(null, null, null, null)]
        public void Should_Return_Failure_When_Parameters_Is_Invalid(string bondType, string bondIndex,
            string isAntecipatedSell, int idCustodyManagerBond)
        {
            var bondsResponse = new GetBondParseResponse();

            _mediator.Send(new GetBondParseQuery { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell })
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var bondParse = new BondParseDto { BondType = bondType, BondIndex = bondIndex, IdCustodyManagerBond = idCustodyManagerBond, IsAntecipatedSell = isAntecipatedSell };
            var createBondRequest = new CreateBondParseCommand { BondParse = bondParse };

            var response = _createHandler.Handle(createBondRequest, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
            });
        }

        [Test]
        public void Should_Throws_Exception_When_Command_is_Null()
        {
            var response = _createHandler.Handle(null, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
                Assert.IsTrue(response.Result.Messages.Contains("A requisição não pode ser nula."));
            });
        }

        [Test]
        [TestCase("ABC", "BCA", "CAB", 10)]
        public void Should_Return_Failure_When_Already_Has_Bond_Parse_Registry(string bondType, string bondIndex, string isAntecipatedSell, int idCustodyManagerBond)
        {
            var bondParse = new BondParseDto { BondIndex = bondIndex, BondType = bondType, IdCustodyManagerBond = idCustodyManagerBond, IsAntecipatedSell = isAntecipatedSell };
            var bondParseFound = new Infohub.Parse.Application.Query.Dtos.BondParseDto
            {
                BondIndex = bondIndex, BondType = bondType, IdCustodyManagerBond = idCustodyManagerBond,
                IsAntecipatedSell = isAntecipatedSell
            };
            var bondsResponse = new GetBondParseResponse {BondParse = bondParseFound};

            _mediator.Send(new GetBondParseQuery { BondIndex = bondIndex, BondType = bondType, IsAntecipatedSell = isAntecipatedSell })
                .ReturnsForAnyArgs(Response<GetBondParseResponse>.Ok(bondsResponse));

            var createIndexRequest = new CreateBondParseCommand { BondParse = bondParse };
            var response = _createHandler.Handle(createIndexRequest, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsFailure);
                Assert.IsFalse(response.Result.IsSuccess);
                Assert.IsNotEmpty(response.Result.Messages);
            });
        }
    }
}
