using Easynvest.Infohub.Parse.Domain.Entities;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Domain.Models
{
    public class IssuerParseTests
    {
        readonly string msgIssuerNameCetip = "O campo Emissor não pode ser nulo.";
        readonly string msgIssuerNameCustodyManager = "O campo Mnemônico não pode ser nulo.";

        [Test]
        [TestCase("AAA", "AAA")]
        public void Should_Return_Success_When_Parameters_Is_Valid(string issuerNameCetip, string issuerNameCustodyManager)
        {
        }

        [Test]
        [TestCase(null, "AAA")]
        public void Should_Return_Failure_When_IssuerNameCetip_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
        }

        [Test]
        [TestCase("AAA", null)]
        public void Should_Return_Failure_When_IssuerNameCustodyManager_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
        }

        [Test]
        [TestCase(null, null)]
        public void Should_Return_Failure_When_IsAntecipatedSell_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
        }
    }
}
