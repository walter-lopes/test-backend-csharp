using Easynvest.Infohub.Parse.Domain.Entities;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Domain.Models
{
    public class BondParseTests
    {
        readonly string msgBondType = "O campo Tipo IF não pode ser nulo.";
        readonly string msgBondIndex = "O campo Indexador não pode ser nulo.";
        readonly string msgIsAntecipatedSell = "O campo Condição Resgate não pode ser nulo.";

        [Test]
        [TestCase("AAA", "AAA", "AAA")]
        public void Should_Return_Success_When_Parameters_Is_Valid(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsSuccess);
                Assert.IsFalse(index.IsFailure);
                Assert.IsEmpty(index.Messages);
            });
        }

        [Test]
        [TestCase(null, "AAA", "AAA")]
        public void Should_Return_Failure_When_BondType_Is_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgBondType));
            });
        }

        [Test]
        [TestCase("AAA", null, "AAA")]
        public void Should_Return_Failure_When_BondIndex_Is_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgBondIndex));
            });
        }

        [Test]
        [TestCase("AAA", "null", null)]
        public void Should_Return_Failure_When_IsAntecipatedSell_Is_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgIsAntecipatedSell));
            });
        }

        [Test]
        [TestCase(null, null, "AAA")]
        public void Should_Return_Failure_When_BondType_And_BondIndex_Are_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgBondIndex));
                Assert.IsTrue(index.Messages.Contains(msgBondType));
            });
        }

        [Test]
        [TestCase("AAA", null, null)]
        public void Should_Return_Failure_When_BondIndex_And_IsAntecipatedSell_Are_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgBondIndex));
                Assert.IsTrue(index.Messages.Contains(msgIsAntecipatedSell));
            });
        }

        [Test]
        [TestCase(null, null, null)]
        public void Should_Return_Failure_When_All_Parameters_Are_Null(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var index = BondParse.Create(bondType, bondIndex, isAntecipatedSell, Arg.Any<int>());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(index);
                Assert.IsTrue(index.IsFailure);
                Assert.IsFalse(index.IsSuccess);
                Assert.IsNotEmpty(index.Messages);
                Assert.IsTrue(index.Messages.Contains(msgBondType));
                Assert.IsTrue(index.Messages.Contains(msgBondIndex));
                Assert.IsTrue(index.Messages.Contains(msgIsAntecipatedSell));
            });
        }

    }
}
