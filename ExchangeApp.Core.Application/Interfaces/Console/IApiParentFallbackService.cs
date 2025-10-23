using ExchangeApp.Core.Application.Dtos;

namespace ExchangeApp.Core.Application.Interfaces.Console
{
    public interface IApiParentFallbackService
    {
        Task<object> GetBestOfferAsync(ExchangeRequestDto request);
    }
}