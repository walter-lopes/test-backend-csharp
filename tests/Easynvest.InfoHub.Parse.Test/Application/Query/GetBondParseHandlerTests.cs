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
    public class GetBondParseHandlerTests
    {
        private ILogger<GetBondParseHandler> _logger;
        private IBondParseRepository _bondParseRepository;
        private GetBondParseHandler _getBondParseHandler;
        private AuthenticatedUser _authenticatedUser;
       

        [SetUp]
        public void SetUp()
        {
            _bondParseRepository = Substitute.For<IBondParseRepository>();
            _logger = Substitute.For<ILogger<GetBondParseHandler>>();
            _authenticatedUser = new AuthenticatedUser(Substitute.For<IHttpContextAccessor>());
            
            _getBondParseHandler = new GetBondParseHandler(_logger, _authenticatedUser, _bondParseRepository);
        }

        [Test]
        [TestCase("ABC", "BCA", "CAB")]
        public void Should_Return_Response_Success_With_No_Exception(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var response = _getBondParseHandler.Handle(new GetBondParseQuery { BondType = bondType, BondIndex = bondIndex, IsAntecipatedSell = isAntecipatedSell }, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsTrue(response.Result.IsSuccess);
                Assert.IsFalse(response.Result.IsFailure);
                Assert.IsEmpty(response.Result.Messages);
            });
        }

        [Test]
        [TestCase("ABC", "BCA", "CAB")]
        public void Should_Return_Response_Failure_With_Exception(string bondType, string bondIndex, string isAntecipatedSell)
        {
            _bondParseRepository.When(x => x.GetBy(bondType, bondIndex, isAntecipatedSell)).Do(x => throw new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _getBondParseHandler.Handle(new GetBondParseQuery { BondType = bondType, BondIndex = bondIndex, IsAntecipatedSell = isAntecipatedSell }, CancellationToken.None));
        }
    }
}
