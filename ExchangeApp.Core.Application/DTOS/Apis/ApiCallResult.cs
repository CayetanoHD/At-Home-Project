using ExchangeApp.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.DTOS.Apis
{
    public class ApiCallResult
    {
        public string Source { get; set; } = null!;
        public Result<ExchangeResponseDto> Response { get; set; } = null!;
        public TimeSpan ResponseTime { get; set; }
        public string? Error { get; set; }
    }

}
