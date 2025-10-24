using ExchangeApp.Core.Application.DTOS.Apis.API;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using ExchangeApp.Core.Application.Services.Api1;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ExchangeApp.Controllers
{
    [ApiController]
    [Route("api/api1")]
    public class Api1Controller : ControllerBase
    {
        private readonly IApi1Service _service;

        public Api1Controller(IApi1Service api1Service)
        {
               _service = api1Service; 
        }

        [HttpPost("convert")]
        [ProducesResponseType(typeof(Api1Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Convert([FromBody] Api1Request request)
        {
            var result = _service.Convert(request.From, request.To, request.Value);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }




    }

}
