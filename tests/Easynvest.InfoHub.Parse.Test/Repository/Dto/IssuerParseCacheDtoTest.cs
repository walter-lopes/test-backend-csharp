using Easynvest.Infohub.Parse.Infra.Data.Repositories.Cache.Dto;
using FluentAssertions;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Repository.Dto
{
    public class IssuerParseCacheDtoTest
    {
        [Test]
        [TestCase("DI", "CDI")]
        [TestCase("DI", "-")]
        public void Should_Mapper_To_Domain(string issuerNameCetip, string issuerNameCustodyManager)
        {
            IssuerParseCacheDto dto = new IssuerParseCacheDto(issuerNameCustodyManager, issuerNameCetip);

            var issuerParse = dto.ToDomain();

            issuerParse.IssuerNameCetip.Should().Be(issuerNameCetip);
            issuerParse.IssuerNameCustodyManager.Should().Be(issuerNameCustodyManager);
        }
    }
}
