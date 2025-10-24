using ExchangeApp.Core.Application.DTOS.Apis.Api2;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ExchangeApp2.Controllers
{
    [ApiController]
    [Route("api/api2")]
    public class Api2Controller : ControllerBase
    {
        private readonly IApi2Service _service;

        public Api2Controller(IApi2Service api2Service)
        {
            _service = api2Service;
                
        }

        [HttpPost("convert")]
        [ProducesResponseType(typeof(Api2Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Convert([FromBody] Api2Request request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.SourceCurrency) ||
                    string.IsNullOrWhiteSpace(request.TargetCurrency) ||
                    request.Quantity <= 0)
                {
                    return BadRequest("Invalid input.");
                }

                var result = _service.Convert(request.SourceCurrency, request.TargetCurrency, request.Quantity);

                if (result.IsFailure)
                    return BadRequest(result.Error);

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid request: {ex.Message}");
            }
        }






    }

}
