using Easynvest.Infohub.Parse.Api.Controllers;
using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using BondParseDto = Easynvest.Infohub.Parse.Application.Command.Dtos.BondParseDto;

namespace Easynvest.InfoHub.Parse.Test.Api.Controllers
{
    public class BondParserControllerTests
    {
        private readonly BondParserController _controller;
        private readonly IMediator _mediator;

        public BondParserControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new BondParserController(_mediator);
        }

        [Test]
        public void Should_Return_Response_When_Get_Bonds_Is_Success()
        {
            var getRequest = new GetBondsParseQuery();
            var getResponse = new GetBondsParseResponse();

            _mediator.Send(getRequest).ReturnsForAnyArgs(Response<GetBondsParseResponse>.Ok(getResponse));

            var response = _controller.GetBonds();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Get_Bonds_Is_Failure()
        {
            var getRequest = new GetBondsParseQuery();
            _mediator.Send(getRequest).ReturnsForAnyArgs(Response<GetBondsParseResponse>.Fail("A requisição não pode ser nula."));

            var response = _controller.GetBonds();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Post_Bond_Is_Success()
        {
            _mediator.Send(new CreateBondParseCommand()).ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var response = _controller.CreateBond(new BondParseDto());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Post_Bond_Is_Failure()
        {
            _mediator.Send(new CreateBondParseCommand()).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var response = _controller.CreateBond(new BondParseDto());

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Put_Bond_Is_Success()
        {
            _mediator.Send(new UpdateBondParseCommand()).ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var request = new BondParseDto();
            var response = _controller.UpdateBond(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Put_Bond_Is_Failure()
        {
            _mediator.Send(new UpdateBondParseCommand()).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var request = new BondParseDto();
            var response = _controller.UpdateBond(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        [TestCase("AAA", "BBB", "CCC")]
        public void Should_Return_Response_When_Delete_Bond_Is_Success(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var deleteRequest = new DeleteBondParseCommand { BondType = bondType, BondIndex = bondIndex, IsAntecipatedSell = isAntecipatedSell };

            _mediator.Send(deleteRequest).ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var response = _controller.DeleteBond(bondType, bondIndex, isAntecipatedSell);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
            });
        }

        [Test]
        [TestCase("AAA", "BBB", "CCC")]
        public void Should_Return_Response_When_Delete_Bond_Is_Failure(string bondType, string bondIndex, string isAntecipatedSell)
        {
            var deleteRequest = new DeleteBondParseCommand { BondType = bondType, BondIndex = bondIndex, IsAntecipatedSell = isAntecipatedSell };
            _mediator.Send(deleteRequest).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var response = _controller.DeleteBond(bondType, bondIndex, isAntecipatedSell);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Failure_When_Delete_Parameters_Are_Null()
        {
            var response = _controller.DeleteBond(null, null, null);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }
    }
}
