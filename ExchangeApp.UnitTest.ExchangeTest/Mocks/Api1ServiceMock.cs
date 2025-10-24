using ExchangeApp.Core.Application.DTOS.Apis.API;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Test.ExchangeTest.Services.Mocks
{

    public static class Api1ServiceMock
    {
        public static Mock<IApi1Service> GetMock()
        {
            var mock = new Mock<IApi1Service>();

            // 🟢 PRIMERO: fallback general (mismo tipo de moneda o no soportado)
            mock.Setup(s => s.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                {
                    if (from.Equals(to, StringComparison.OrdinalIgnoreCase))
                        return Result<Api1Response>.Success(new Api1Response { ConvertedAmount = amount });

                    var allowed = new[] { "USD", "DOP", "EUR" };
                    if (!allowed.Contains(from.ToUpper()) || !allowed.Contains(to.ToUpper()))
                        return Result<Api1Response>.Failure("Only USD, DOP, and EUR are supported.");

                    // fallback (si no hay tasa específica)
                    return Result<Api1Response>.Success(new Api1Response { ConvertedAmount = amount });
                });

            // 🔹 USD → DOP
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount * 59.00m, 2)
                    }));

            // 🔹 DOP → USD
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount / 59.00m, 2)
                    }));

            // 🔹 EUR → DOP
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount * 63.50m, 2)
                    }));

            // 🔹 DOP → EUR
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount / 63.50m, 2)
                    }));

            // 🔹 USD → EUR
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount * 0.92m, 2)
                    }));

            // 🔹 EUR → USD
            mock.Setup(s => s.Convert(
                It.Is<string>(f => f.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(t => t.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount / 0.92m, 2)
                    }));

            return mock;
        }
    }
}




