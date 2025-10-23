using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeAppManager.Controllers
{
    [ApiController]
    [Route("api/exchange")]
    public class ExchangeController : ControllerBase
    {
        private readonly IBestRateService _bestRateService;

        public ExchangeController(IBestRateService bestRateService)
        {
            _bestRateService = bestRateService;
        }

        [HttpPost("best-offer")]
        public async Task<IActionResult> GetBestOffer([FromBody] ExchangeRequestDto request)
        {
            var result = await _bestRateService.GetBestRateAsync(request);
            if (result == null)
                return StatusCode(503, "All providers unavailable");

            return Ok(result);
        }
    }
}
