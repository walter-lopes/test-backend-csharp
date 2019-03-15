using Easynvest.Infohub.Parse.Application.Query.Handlers;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading;
using Easynvest.Infohub.Parse.Domain.Interfaces;

namespace Easynvest.InfoHub.Parse.Test.Application.Query
{
    public class GetBondsParseHandlerTests
    {
        private ILogger<GetBondsParseHandler> _logger;
        private IBondParseRepository _bondParserRepository;
        private GetBondsParseHandler _getBondsHandler;
        private AuthenticatedUser _authenticatedUser;
        private Infohub.Parse.Infra.CrossCutting.Log.Logger _log;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<GetBondsParseHandler>>();
            _bondParserRepository = Substitute.For<IBondParseRepository>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            _log = new Infohub.Parse.Infra.CrossCutting.Log.Logger(_authenticatedUser);
            _getBondsHandler = new GetBondsParseHandler(_logger, _authenticatedUser, _bondParserRepository);
        }

        [Test]
        public void Should_Return_Response_Failure_With_Exception()
        {
            _bondParserRepository.When(x => x.GetAll()).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _getBondsHandler.Handle(new GetBondsParseQuery(), CancellationToken.None));
        }

        [Test]
        public void Should_Return_Response_Success_With_No_Exception()
        {
            var response = _getBondsHandler.Handle(new GetBondsParseQuery(), CancellationToken.None);

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
