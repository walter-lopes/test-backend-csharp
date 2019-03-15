using Easynvest.Infohub.Parse.Application.Command.Commands;
using Easynvest.Infohub.Parse.Application.Command.Dtos;
using Easynvest.Infohub.Parse.Application.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
namespace Easynvest.Infohub.Parse.Api.Controllers
{
    [Produces("application/json")]
    [Route("Issuer")]
    [ApiController]
    public class IssuerParserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssuerParserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetIssuers()
        {
            var response = await _mediator.Send(new GetIssuersParseQuery());

            if (response.IsFailure)
                return BadRequest(response);

            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreateIssuer([FromBody] IssuerParseDto request)
        {
            var response = await _mediator.Send(new CreateIssuerParseCommand { IssuerParse = request });

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateIssuer([FromBody] IssuerParseDto request)
        {
            var response = await _mediator.Send(new UpdateIssuerParseCommand { IssuerParse = request });

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }

        [HttpDelete]
        [Route("{issuerNameCetip}")]
        public async Task<IActionResult> DeleteIssuer([FromRoute] string issuerNameCetip)
        {
            var response = await _mediator.Send(new DeleteIssuerParseCommand { IssuerNameCetip = issuerNameCetip });

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }
    }
}
