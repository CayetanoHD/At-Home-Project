using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Services.Api3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.UnitTest.ExchangeTest.Services
{
    public class Api3ServiceTests
    {
        private readonly IApi3Service _service;

        public Api3ServiceTests()
        {
            var loggerMock = new Mock<ILogger<Api3Service>>();
            _service = new Api3Service(loggerMock.Object);
        }

        /// <summary>
        /// 1. Convierte USD a DOP según la tasa simulada de API3.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenUsdToDop()
        {
            var result = _service.Convert("USD", "DOP", 10);

            result.IsSuccess.Should().BeTrue();
            result.Value.Result.Should().Be(585.00m);
        }

        /// <summary>
        /// 2. Convierte DOP a USD según la tasa simulada de API3.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenDopToUsd()
        {
            var result = _service.Convert("DOP", "USD", 585);

            result.IsSuccess.Should().BeTrue();
            result.Value.Result.Should().Be(10.00m);
        }

        /// <summary>
        /// 3. Convierte EUR a DOP según la tasa simulada de API3 (el euro más caro).
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenEurToDop()
        {
            var result = _service.Convert("EUR", "DOP", 2);

            result.IsSuccess.Should().BeTrue();
            result.Value.Result.Should().Be(130.00m);
        }

        /// <summary>
        /// 4. Devuelve el mismo monto si la moneda de origen y destino son iguales.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnSameAmount_WhenFromAndToAreEqual()
        {
            var result = _service.Convert("USD", "USD", 100);

            result.IsSuccess.Should().BeTrue();
            result.Value.Result.Should().Be(100.00m);
        }

        /// <summary>
        /// 5. Conversiones varias para probar fallback y mayúsculas/minúsculas.
        /// </summary>
        [Theory]
        [InlineData("usd", "dop", 5, 292.50)]
        [InlineData("EUR", "USD", 9.50, 10.00)]
        [InlineData("DOP", "EUR", 130, 2.00)]
        [InlineData("USD", "EUR", 10, 9.50)]
        public void Convert_ShouldHandleVariousCurrencies_WithFallback(string from, string to, decimal amount, decimal expected)
        {
            var result = _service.Convert(from, to, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Result.Should().Be(expected);
        }

        /// <summary>
        /// 6. Devuelve error cuando la moneda de origen no está soportada.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenCurrencyNotSupported()
        {
            var result = _service.Convert("BTC", "DOP", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// 7. Devuelve error cuando ambas monedas no están soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenBothCurrenciesUnsupported()
        {
            var result = _service.Convert("BTC", "ETH", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }
    }
}
