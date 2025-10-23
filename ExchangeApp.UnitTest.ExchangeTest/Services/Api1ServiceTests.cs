
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Services.Api1;
using ExchangeApp.Test.ExchangeTest.Services.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeApp.Test.ExchangeTest.Services
{

    public class Api1ServiceTests
    {
        private readonly IApi1Service _service;

        public Api1ServiceTests()
        {
            // Logger mock necesario para instanciar el servicio real
            var loggerMock = new Mock<ILogger<Api1Service>>();
            _service = new Api1Service(loggerMock.Object);
        }

        /// <summary>
        /// 1. Convierte USD a DOP según la tasa simulada de Api1.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenUsdToDop()
        {
            var result = _service.Convert("USD", "DOP", 10);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(590.00m);
        }

        /// <summary>
        /// 2. Convierte DOP a USD según la tasa simulada de Api1.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenDopToUsd()
        {
            var result = _service.Convert("DOP", "USD", 590);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(10.00m);
        }

        /// <summary>
        /// 3. Devuelve el mismo monto si la moneda de origen y destino son iguales.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnSameAmount_WhenFromAndToAreEqual()
        {
            var result = _service.Convert("USD", "USD", 100);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(100.00m);
        }

        /// <summary>
        /// 4. Conversiones varias usando las tasas reales de Api1.
        /// </summary>
        [Theory]
        [InlineData("USD", "EUR", 10, 9.20)]   // 10 * 0.92
        [InlineData("EUR", "USD", 10, 10.87)]  // 10 / 0.92
        [InlineData("EUR", "DOP", 2, 127.00)]  // 2 * 63.50
        [InlineData("DOP", "EUR", 10, 0.16)]   // 10 / 63.50
        public void Convert_ShouldHandleVariousCurrencies_WithRealRates(string from, string to, decimal amount, decimal expected)
        {
            var result = _service.Convert(from, to, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(expected);
        }

        /// <summary>
        /// 5. Error si la moneda de origen no está soportada.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenCurrencyNotSupported()
        {
            var result = _service.Convert("BTC", "DOP", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// 6. Error si ambas monedas no están soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenBothCurrenciesUnsupported()
        {
            var result = _service.Convert("BTC", "ETH", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// 7. Verifica que los códigos de moneda sean tratados sin distinguir mayúsculas/minúsculas.
        /// </summary>
        [Fact]
        public void Convert_ShouldBeCaseInsensitive_ForCurrencyCodes()
        {
            var result = _service.Convert("usd", "dop", 1);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(59.00m);
        }
    }
}



