using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Results;

namespace ExchangeApp.Core.Application.Interfaces.Apis
{
    public interface IApi3Service
    {
        Result<Api3Response> Convert(string from, string to, decimal amount);
    }
}