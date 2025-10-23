using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Test.ExchangeTest.Mocks
{
    public static class Api1ServiceMock
    {
        public static Mock<IApi1Service> GetMock()
        {
            var mock = new Mock<IApi1Service>();

            // 🔹 Mock de conversión válida USD → DOP
            mock.Setup(s => s.Convert("USD", "DOP", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount * 59.00m, 2)
                    })
                );

            // 🔹 Mock de conversión válida DOP → USD
            mock.Setup(s => s.Convert("DOP", "USD", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = Math.Round(amount / 59.00m, 2)
                    })
                );

            // 🔹 Mock para cualquier combinación no soportada
            mock.Setup(s => s.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                {
                    var allowed = new[] { "USD", "DOP", "EUR" };
                    if (!allowed.Contains(from.ToUpper()) || !allowed.Contains(to.ToUpper()))
                        return Result<Api1Response>.Failure("Only USD, DOP, and EUR are supported.");

                    // fallback default (mismo valor si no se cumple ninguna tasa simulada)
                    return Result<Api1Response>.Success(new Api1Response { ConvertedAmount = amount });
                });

            return mock;
        }
    }
}
