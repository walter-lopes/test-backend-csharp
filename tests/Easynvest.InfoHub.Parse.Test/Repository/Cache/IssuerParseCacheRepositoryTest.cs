using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache.Dto;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Easynvest.InfoHub.Parse.Test.Repository.Cache
{
    [TestFixture]
    public class IssuerParseCacheRepositoryTest
    {
        private IIssuerParseRepository _issuerParseRepository;
        private IssuerParseCacheRepository _issuerParseCacheRepository;
        private ICache _cache;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<ICache>();
            _issuerParseRepository = Substitute.For<IIssuerParseRepository>();
            _issuerParseCacheRepository = new IssuerParseCacheRepository(_issuerParseRepository, _cache);
        }

        [Test]
        [TestCase("DI", "CDI")]
        [TestCase("DI", "-")]
        public void Should_Get_Value_From_Cache(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var key = $"IssuerParse:{issuerNameCetip}";

       
            _cache.Get<IssuerParseCacheDto>(key).Returns(new IssuerParseCacheDto(issuerNameCustodyManager, issuerNameCetip));

            var issuer = _issuerParseCacheRepository.GetBy(issuerNameCetip);

            issuer.Result.Should().NotBeNull();
            issuer.Result.IssuerNameCetip.Should().Be(issuerNameCetip);
            issuer.Result.IssuerNameCustodyManager.Should().Be(issuerNameCustodyManager);
        }
    }
}
