using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;
using ExchangeApp.Core.Application.Results;

namespace ExchangeApp.Core.Application.Interfaces.ExternalApi
{
    public interface IApi3ExchangeService
    {
        Task<Result<ExchangeResponseDto>> GetRateAsync(ExchangeRequestDto request);
    }
}