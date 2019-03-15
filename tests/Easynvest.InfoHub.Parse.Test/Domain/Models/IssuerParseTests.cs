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
            var index = IssuerParse.Create(issuerNameCustodyManager, issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsSuccess);
                Assert.IsFalse(index.IsFailure);
                Assert.IsEmpty(index.Messages);
            });
        }

        [Test]
        [TestCase(null, "AAA")]
        public void Should_Return_Failure_When_IssuerNameCetip_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var index = IssuerParse.Create(issuerNameCustodyManager, issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgIssuerNameCetip));
            });
        }

        [Test]
        [TestCase("AAA", null)]
        public void Should_Return_Failure_When_IssuerNameCustodyManager_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var index = IssuerParse.Create(issuerNameCustodyManager, issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgIssuerNameCustodyManager));
            });
        }

        [Test]
        [TestCase(null, null)]
        public void Should_Return_Failure_When_IsAntecipatedSell_Is_Null(string issuerNameCetip, string issuerNameCustodyManager)
        {
            var index = IssuerParse.Create(issuerNameCustodyManager, issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgIssuerNameCustodyManager));
                Assert.IsTrue(index.Messages.Contains(msgIssuerNameCetip));
            });
        }
    }
}
