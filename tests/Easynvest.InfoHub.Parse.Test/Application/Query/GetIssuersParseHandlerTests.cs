using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class GetIssuersParseHandlerTests
    {
        private ILogger<GetIssuersParseHandler> _logger;
        private IIssuerParseRepository _issuerParseRepository;
        private GetIssuersParseHandler _getIssuersHandler;
        private AuthenticatedUser _authenticatedUser;


        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<GetIssuersParseHandler>>();
            _issuerParseRepository = Substitute.For<IIssuerParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            _getIssuersHandler = new GetIssuersParseHandler(_logger, _authenticatedUser, _issuerParseRepository);
        }

        [Test]
        public void Should_Return_Response_Failure_With_Exception()
        {
            _issuerParseRepository.When(x => x.GetAll()).Do(x => throw new Exception());

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
