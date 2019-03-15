using System.Collections.Generic;
using Easynvest.Infohub.Parse.Api.Controllers;
using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Dtos;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using Easynvest.Infohub.Parse.Application.Query.Responses;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Responses;
using MediatR;
using NSubstitute;
using NUnit.Framework;

namespace Easynvest.InfoHub.Parse.Test.Api.Controllers
{
    public class IssuerParserControllerTests
    {

        private readonly IssuerParserController _controller;
        private readonly IMediator _mediator;

        public IssuerParserControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new IssuerParserController(_mediator);
        }

        [Test]
        public void Should_Return_Response_When_Get_Bonds_Is_Success()
        {
            var getRequest = new GetIssuersParseQuery();
            var getResponse = new GetIssuersParseResponse();

            _mediator.Send(getRequest).ReturnsForAnyArgs(Response<GetIssuersParseResponse>.Ok(getResponse));

            var response = _controller.GetIssuers();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Get_Bonds_Is_Failure()
        {
            var getRequest = new GetIssuersParseQuery();
            var getResponse = new GetIssuersParseResponse();
            _mediator.Send(getRequest).ReturnsForAnyArgs(Response<GetIssuersParseResponse>.Fail("A requisição não pode ser nula."));

            var response = _controller.GetIssuers();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Post_Bond_Is_Success()
        {
            var request = new IssuerParseDto();
            _mediator.Send(new CreateIssuerParseCommand { IssuerParse = request })
                .ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var response = _controller.CreateIssuer(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Post_Bond_Is_Failure()
        {
            var request = new IssuerParseDto();
            _mediator.Send(new CreateIssuerParseCommand {IssuerParse = request}).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var response = _controller.CreateIssuer(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Put_Bond_Is_Success()
        {
            var request = new IssuerParseDto();

            _mediator.Send(new UpdateIssuerParseCommand { IssuerParse = request }).ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var response = _controller.UpdateIssuer(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        public void Should_Return_Response_When_Put_Bond_Is_Failure()
        {
            var request = new IssuerParseDto();
            _mediator.Send(new UpdateIssuerParseCommand { IssuerParse = request }).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var response = _controller.UpdateIssuer(request);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        [TestCase("AAA")]
        public void Should_Return_Response_When_Delete_Bond_Is_Success(string issuerNameCetip)
        {
            var deleteCommand = new DeleteIssuerParseCommand { IssuerNameCetip = issuerNameCetip};

            _mediator.Send(deleteCommand).ReturnsForAnyArgs(Response<Unit>.Ok(new Unit()));

            var response = _controller.DeleteIssuer(issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }

        [Test]
        [TestCase("AAA")]
        public void Should_Return_Response_When_Delete_Bond_Is_Failure(string issuerNameCetip)
        {
            var deleteCommand = new DeleteIssuerParseCommand { IssuerNameCetip = issuerNameCetip };

            _mediator.Send(deleteCommand).ReturnsForAnyArgs(Response<Unit>.Fail("A requisição não pode ser nula."));

            var response = _controller.DeleteIssuer(issuerNameCetip);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Result);
            });
        }
    }
}
