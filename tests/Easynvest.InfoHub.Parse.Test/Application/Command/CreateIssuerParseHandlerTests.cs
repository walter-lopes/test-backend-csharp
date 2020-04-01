using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Dtos;
using Easynvest.Infohub.Parse.Application.Command.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
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
    public class CreateIssuerParseHandlerTests
    {
        private ILogger<CreateIssuerParseHandler> _logger;
        private CreateIssuerParseHandler _createHandler;
        private IIssuerParseRepository _issuerParseRepository;
        private AuthenticatedUser _authenticatedUser;

        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<CreateIssuerParseHandler>>();
            _issuerParseRepository = Substitute.For<IIssuerParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());

            _mediator = Substitute.For<IMediator>();
            _createHandler = new CreateIssuerParseHandler(_logger, _authenticatedUser, _issuerParseRepository, _mediator);
        }

        [Test]
        [TestCase("AAA", "BBB")]
        [TestCase("AAA", "-")]
        public void Return_Success_With_Without_Exception_When_Create(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var indexResponse = new GetIssuerParseResponse();

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(indexResponse));

            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };

            var createIndexRequest = new CreateIssuerParseCommand { IssuerParse = issuerParse };
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
        [TestCase("AAA", "")]
        [TestCase("AAA", "-")]
        [TestCase("AAA", "BBB")]
        public void Return_Throws_Exception_When_Create(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerResponse = new GetIssuerParseResponse { IssuerParse = null };

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };
            var createIssuerRequest = new CreateIssuerParseCommand { IssuerParse = issuerParse  };

            _issuerParseRepository.Create(Arg.Any<IssuerParse>()).Returns(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _createHandler.Handle(createIssuerRequest, CancellationToken.None));
        }

        [Test]
        [TestCase(null, "BBB")]
        [TestCase(null, null)]
        public void Should_Return_Failure_When_Parameters_Is_Invalid(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };
            var createIssuerRequest = new CreateIssuerParseCommand { IssuerParse = issuerParse };

            var response = _createHandler.Handle(createIssuerRequest, CancellationToken.None);

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
        [TestCase("AAA", "BBB")]
        public void Should_Return_Failure_When_Already_Has_Issuer_Parse_Registry(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerParseFound = new Infohub.Parse.Application.Query.Dtos.IssuerParseDto
            { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };
            var issuerResponse = new GetIssuerParseResponse { IssuerParse = issuerParseFound };

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };
            var createIndexRequest = new CreateIssuerParseCommand { IssuerParse = issuerParse };
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
