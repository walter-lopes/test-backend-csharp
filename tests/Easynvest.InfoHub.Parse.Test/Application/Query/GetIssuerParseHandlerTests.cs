using System;
using System.Threading;
using Easynvest.Infohub.Parse.Application.Query.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Application.Query
{
    public class GetIssuerParseHandlerTests
    {
        private ILogger<GetIssuerParseHandler> _logger;
        private IIssuerParseRepository _issuerParseRepository;
        private GetIssuerParseHandler _getIssuerParseHandler;
        private AuthenticatedUser _authenticatedUser;
       

        [SetUp]
        public void SetUp()
        {
            _issuerParseRepository = Substitute.For<IIssuerParseRepository>();
            _logger = Substitute.For<ILogger<GetIssuerParseHandler>>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _getIssuerParseHandler = new GetIssuerParseHandler(_logger, _authenticatedUser, _issuerParseRepository);
        }

        [Test]
        [TestCase("ABC")]
        public void Should_Return_Response_Success_With_No_Exception(string issuerNemCetip)
        {
            var response = _getIssuerParseHandler.Handle(new GetIssuerParseQuery { IssuerNameCetip = issuerNemCetip }, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }

        [Test]
        [TestCase("ABC")]
        public void Should_Return_Response_Failure_With_Exception(string issuerNemCetip)
        {
            _issuerParseRepository.When(x => x.GetBy(issuerNemCetip)).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _getIssuerParseHandler.Handle(new GetIssuerParseQuery { IssuerNameCetip = issuerNemCetip }, CancellationToken.None));
        }
    }
}
