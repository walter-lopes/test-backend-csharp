using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Handlers;
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
using System;
using System.Threading;
using Easynvest.Infohub.Parse.Application.Query.Dtos;
using Easynvest.Infohub.Parse.Domain.Entities;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;

namespace Easynvest.InfoHub.Parse.Test.Application.Command
{
    public class DeleteIssuerParseHandlerTest
    {
        private ILogger<DeleteIssuerParseHandler> _logger;
        private DeleteIssuerParseHandler _deleteIssuerHandler;
        private Func<RepositoryType, IIssuerParseRepository> _issuerParseRepository;
        private AuthenticatedUser _authenticatedUser;
       
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<DeleteIssuerParseHandler>>();
            _issuerParseRepository = Substitute.For<Func<RepositoryType, IIssuerParseRepository>>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _mediator = Substitute.For<IMediator>();
            _deleteIssuerHandler = new DeleteIssuerParseHandler(_logger, _authenticatedUser, _issuerParseRepository, _mediator);
        }

        [Test]
        [TestCase("AAA")]
        public void Should_Return_Success_When_Delete(string issuerNameCetip)
        {
            var issuerParseFound = new IssuerParseDto { IssuerNameCetip = issuerNameCetip };
            var issuerResponse = new GetIssuerParseResponse { IssuerParse = issuerParseFound };

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            var deleteBondParseCommand = new DeleteIssuerParseCommand { IssuerNameCetip = issuerNameCetip };
            var response = _deleteIssuerHandler.Handle(deleteBondParseCommand, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }

        [Test]
        [TestCase("AAA")]
        [TestCase("")]
        [TestCase("-")]
        public void Should_Throw_Exception_When_Delete(string issuerNameCetip)
        {
            var issuerParseFound = new IssuerParseDto { IssuerNameCetip = issuerNameCetip };
            var issuerResponse = new GetIssuerParseResponse { IssuerParse = issuerParseFound };

            _mediator.Send(new GetIssuerParseQuery { IssuerNameCetip = issuerNameCetip })
                .ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            _mediator.Send(new GetIssuerParseQuery()).ReturnsForAnyArgs(Response<GetIssuerParseResponse>.Ok(issuerResponse));

            var deleteIssuerParseCommand = new DeleteIssuerParseCommand { IssuerNameCetip = issuerNameCetip };

            var mock = _issuerParseRepository(RepositoryType.Cache);

            mock.When(x => x.Delete(Arg.Any<IssuerParse>())).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _deleteIssuerHandler.Handle(deleteIssuerParseCommand, CancellationToken.None));
        }

        [Test]
        public void Should_Return_Failure_When_Parameter_Is_Null()
        {
            var request = new DeleteIssuerParseCommand();
            var response = _deleteIssuerHandler.Handle(request, CancellationToken.None);

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
            var response = _deleteIssuerHandler.Handle(null, CancellationToken.None);

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
