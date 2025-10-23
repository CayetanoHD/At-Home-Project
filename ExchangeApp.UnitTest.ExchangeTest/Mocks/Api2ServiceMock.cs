using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.UnitTest.ExchangeTest.Mocks
{
    public static class Api2ServiceMock
    {
        /// <summary>
        /// Crea un mock de IApi2Service con conversiones simuladas y validaciones.
        /// </summary>
        public static Mock<IApi2Service> GetMock()
        {
            var mock = new Mock<IApi2Service>();
            var allowed = new[] { "USD", "DOP", "EUR" };

            // 🔹 Conversiones específicas
            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount * 60.50m, 2) }));

            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount / 60.50m, 2) }));

            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount * 64.00m, 2) }));

            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("DOP", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount / 64.00m, 2) }));

            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount * 0.93m, 2) }));

            mock.Setup(s => s.Convert(It.Is<string>(f => f.Equals("EUR", StringComparison.OrdinalIgnoreCase)),
                                      It.Is<string>(t => t.Equals("USD", StringComparison.OrdinalIgnoreCase)),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api2Response>.Success(new Api2Response { Total = Math.Round(amount / 0.93m, 2) }));

            // 🔹 Fallback general (misma moneda o monedas no soportadas)
            mock.Setup(s => s.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                {
                    if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                        return Result<Api2Response>.Failure("Invalid currency input.");

                    if (from.Equals(to, StringComparison.OrdinalIgnoreCase))
                        return Result<Api2Response>.Success(new Api2Response { Total = amount });

                    if (!allowed.Contains(from.ToUpper()) || !allowed.Contains(to.ToUpper()))
                        return Result<Api2Response>.Failure("Only USD, DOP, and EUR are supported.");

                    return Result<Api2Response>.Success(new Api2Response { Total = amount });
                });

            return mock;
        }

    }
}
