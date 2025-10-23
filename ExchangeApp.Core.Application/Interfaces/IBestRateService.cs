using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;

namespace ExchangeApp.Core.Application.Interfaces
{
    public interface IBestRateService
    {
        Task<object> GetBestRateAsync(ExchangeRequestDto request);
    }
}