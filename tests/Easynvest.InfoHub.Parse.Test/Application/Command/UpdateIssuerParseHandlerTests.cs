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
    public class UpdateIssuerParseHandlerTests
    {
        private ILogger<UpdateIssuerParseHandler> _logger;
        private UpdateIssuerParseHandler _updateIssuerHandler;
        private Func<RepositoryType, IIssuerParseRepository> _issuerParseRepository;
        private AuthenticatedUser _authenticatedUser;
       
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<UpdateIssuerParseHandler>>();
            _issuerParseRepository = Substitute.For<Func<RepositoryType, IIssuerParseRepository>>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _mediator = Substitute.For<IMediator>();
            _updateIssuerHandler = new UpdateIssuerParseHandler(_logger, _authenticatedUser, _issuerParseRepository, _mediator);
        }

        [Test]
        [TestCase("DI", "CDI")]
        [TestCase("DI", "-")]
        public void Should_Throws_Exception_When_Update_Issuer(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerParseFound = new Infohub.Parse.Application.Query.Dtos.IssuerParseDto
                { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager};
            var issuerResponse = new GetIssuerParseResponse {IssuerParse = issuerParseFound};

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager };
            var updateIssuerRequest = new UpdateIssuerParseCommand {IssuerParse = issuerParse};
            var mock = _issuerParseRepository(RepositoryType.Cache);

            mock.When(x => x.Update(Arg.Any<IssuerParse>())).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _updateIssuerHandler.Handle(updateIssuerRequest, CancellationToken.None));
        }

        [Test]
        [TestCase(null, "DI")]
        [TestCase(null, null)]
        public void Should_Return_Failure_When_Update_With_Invalid_Parameters(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerParse = new IssuerParseDto { IssuerNameCetip = issuerNameCetip, IssuerNameCustodyManager = issuerNameCustodyManager};
            var updateBondRequest = new UpdateIssuerParseCommand {IssuerParse = issuerParse};
            var response = _updateIssuerHandler.Handle(updateBondRequest, CancellationToken.None);

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
            var response = _updateIssuerHandler.Handle(null, CancellationToken.None);

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
