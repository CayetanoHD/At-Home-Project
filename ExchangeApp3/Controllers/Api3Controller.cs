using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeApp3.Controllers
{
    [ApiController]
    [Route("api/api3")]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class Api3Controller : ControllerBase
    {
        private readonly IApi3Service _service;

        public Api3Controller(IApi3Service api3Service)
        {
                _service = api3Service;
        }

        [HttpPost("convert")]
        [ProducesResponseType(typeof(Api3Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]                      
        public async Task<IActionResult> Convert([FromBody] Api3Request request)
        {
            if (request == null || string.IsNullOrEmpty(request.From) || string.IsNullOrEmpty(request.To))
                return BadRequest("Invalid request body.");

            var result = _service.Convert(request.From, request.To, request.Amount);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }




}
