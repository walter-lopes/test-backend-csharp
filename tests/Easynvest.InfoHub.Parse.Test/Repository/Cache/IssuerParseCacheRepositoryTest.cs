using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Repositories;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache;
using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache.Dto;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Easynvest.InfoHub.Parse.Test.Repository.Cache
{
    public class IssuerParseCacheRepositoryTest
    {
        private Func<RepositoryType, IIssuerParseRepository> _issuerParseRepository;
        private IssuerParseCacheRepository _issuerParseCacheRepository;

        private ICache _cache;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<ICache>();
            _issuerParseRepository = Substitute.For<Func<RepositoryType, IIssuerParseRepository>>();
            _issuerParseCacheRepository = new IssuerParseCacheRepository(_issuerParseRepository, _cache);
        }

        [Test]
        [TestCase("DI", "CDI")]
        [TestCase("DI", "-")]
        public async Task Should_Get_Value_From_Cache(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var issuerParse = new IssuerParseDto(issuerNameCustodyManager, issuerNameCetip);
            _cache.Get<IssuerParseDto>($"IssuerParse:{issuerNameCetip}").Returns(issuerParse);

            var issuer = await _issuerParseCacheRepository.GetBy(issuerNameCetip);

            issuer.Should().NotBeNull();
            issuer.IssuerNameCetip.Should().Be(issuerNameCetip);
            issuer.IssuerNameCustodyManager.Should().Be(issuerNameCustodyManager);
        }
    }
}
