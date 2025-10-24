using ExchangeApp.Core.Application.DTOS.Apis.API;
using ExchangeApp.Core.Application.Results;

namespace ExchangeApp.Core.Application.Interfaces.Apis
{
    public interface IApi1Service
    {
        Result<Api1Response> Convert(string from, string to, decimal amount);
    }
}