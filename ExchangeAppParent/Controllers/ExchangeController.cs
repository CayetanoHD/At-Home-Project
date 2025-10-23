using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAppParent.Controllers
{
    [ApiController]
    [Route("api/exchange")]
    [Produces("application/json", "application/xml")]
    public class ExchangeController : ControllerBase
    {
        private readonly IBestRateService _bestRateService;

        public ExchangeController(IBestRateService bestRateService)
        {
            _bestRateService = bestRateService;
        }

        [HttpPost("best-offer")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBestOffer([FromBody] ExchangeRequestDto request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            var result = await _bestRateService.GetBestRateAsync(request);

            return Ok(result);
        }



    }

}
