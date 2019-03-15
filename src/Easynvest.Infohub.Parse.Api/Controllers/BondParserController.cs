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
    [Route("Bond")]
    [ApiController]
    public class BondParserController : Controller
    {
        private readonly IMediator _mediator;
        public BondParserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetBonds()
        {
            var response = await _mediator.Send(new GetBondsParseQuery());

            if (response.IsFailure)
                return BadRequest(response);

            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> CreateBond([FromBody] BondParseDto request)
        {
            var response = await _mediator.Send(new CreateBondParseCommand { BondParse = request });

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> UpdateBond([FromBody] BondParseDto request)
        {
            var response = await _mediator.Send(new UpdateBondParseCommand { BondParse = request });

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }

        [HttpDelete]
        [Route("{bondType}/{bondIndex}/{isAntecipated}")]
        public async Task<IActionResult> DeleteBond([FromRoute] string bondType, string bondIndex, string isAntecipated)
        {
            if (bondType is null || bondIndex is null || isAntecipated is null)
            {
                return NotFound();
            }

            var command = new DeleteBondParseCommand { BondType = bondType, BondIndex = bondIndex, IsAntecipatedSell = isAntecipated };

            var response = await _mediator.Send(command);

            if (response.IsFailure)
                return BadRequest(response);

            return NoContent();
        }
    }
}