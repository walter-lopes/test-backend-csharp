using System;
using System.Threading;
using Easynvest.Infohub.Parse.Application.Query.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Application.Query
{
    public class GetIssuersParseHandlerTests
    {
        private ILogger<GetIssuersParseHandler> _logger;
        private IIssuerParseRepository _issuerParserRepository;
        private GetIssuersParseHandler _getIssuersHandler;
        private AuthenticatedUser _authenticatedUser;
        private Infohub.Parse.Infra.CrossCutting.Log.Logger _log;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<GetIssuersParseHandler>>();
            _issuerParserRepository = Substitute.For<IIssuerParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            _log = new Infohub.Parse.Infra.CrossCutting.Log.Logger(_authenticatedUser);
            _getIssuersHandler = new GetIssuersParseHandler(_logger, _authenticatedUser, _issuerParserRepository);
        }

        [Test]
        public void Should_Return_Response_Failure_With_Exception()
        {
            _issuerParserRepository.When(x => x.GetAll()).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _getIssuersHandler.Handle(new GetIssuersParseQuery(), CancellationToken.None));
        }

        [Test]
        public void Should_Return_Response_Success_With_No_Exception()
        {
            var response = _getIssuersHandler.Handle(new GetIssuersParseQuery(), CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }
    }
}
