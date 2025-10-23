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
    public static class Api3ServiceMock
    {
        /// <summary>
        /// Crea un mock de IApi3Service con conversiones simuladas y validaciones.
        /// </summary>
        public static Mock<IApi3Service> GetMock()
        {
            var mock = new Mock<IApi3Service>();

            // 🔹 Mock de USD → DOP (dólar más barato que API2)
            mock.Setup(s => s.Convert("USD", "DOP", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount * 58.50m, 2)
                    }));

            // 🔹 Mock de DOP → USD
            mock.Setup(s => s.Convert("DOP", "USD", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount / 58.50m, 2)
                    }));

            // 🔹 Mock de EUR → DOP (euro más caro)
            mock.Setup(s => s.Convert("EUR", "DOP", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount * 65.00m, 2)
                    }));

            // 🔹 Mock de DOP → EUR
            mock.Setup(s => s.Convert("DOP", "EUR", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount / 65.00m, 2)
                    }));

            // 🔹 Mock de USD → EUR
            mock.Setup(s => s.Convert("USD", "EUR", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount * 0.95m, 2)
                    }));

            // 🔹 Mock de EUR → USD
            mock.Setup(s => s.Convert("EUR", "USD", It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                    Result<Api3Response>.Success(new Api3Response
                    {
                        Result = Math.Round(amount / 0.95m, 2)
                    }));

            // 🔹 Mock cuando From y To son iguales → devuelve el mismo monto
            mock.Setup(s => s.Convert(It.Is<string>(f => f == It.IsAny<string>()),
                                      It.Is<string>(t => t == It.IsAny<string>()),
                                      It.IsAny<decimal>()))
                .Returns((string from, string to, decimal amount) =>
                {
                    if (from.ToUpper() == to.ToUpper())
                        return Result<Api3Response>.Success(new Api3Response { Result = amount });

                    // Validar monedas permitidas
                    var allowed = new[] { "USD", "DOP", "EUR" };
                    if (!allowed.Contains(from.ToUpper()) || !allowed.Contains(to.ToUpper()))
                        return Result<Api3Response>.Failure("Only USD, DOP, and EUR are supported.");

                    // fallback default si no coincide con ninguna simulación
                    return Result<Api3Response>.Success(new Api3Response { Result = amount });
                });

            return mock;
        }
    }
}
