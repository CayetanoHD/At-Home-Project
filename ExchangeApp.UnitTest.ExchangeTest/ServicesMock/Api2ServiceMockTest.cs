using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Services.Api2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace ExchangeApp.UnitTests.ExchangeTest.Services
{
    public class Api2ServiceTests
    {
        private readonly IApi2Service _service;

        public Api2ServiceTests()
        {
            _service = new Api2Service(new Mock<ILogger<Api2Service>>().Object);
        }

        /// <summary>
        /// Verifica que la conversión de USD a DOP devuelva el monto convertido correctamente según la tasa de API2.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenUsdToDop()
        {
            var result = _service.Convert("USD", "DOP", 10);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(605.00m); // tasa simulada API2
        }

        /// <summary>
        /// Verifica que la conversión falle cuando se usan monedas no soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenCurrencyNotSupported()
        {
            var result = _service.Convert("BTC", "DOP", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// Verifica que la conversión de DOP a USD devuelva el monto correcto según la tasa de API2.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenDopToUsd()
        {
            var result = _service.Convert("DOP", "USD", 605);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(10.00m); // inversa de 60.50
        }

        /// <summary>
        /// Verifica que la conversión de EUR a DOP devuelva el monto correcto según la tasa de API2.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenEurToDop()
        {
            var result = _service.Convert("EUR", "DOP", 2);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(128.00m); // 2 * 64.00
        }

        /// <summary>
        /// Verifica que si la moneda de origen y destino son iguales, se devuelva el mismo monto.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnSameAmount_WhenFromAndToAreEqual()
        {
            var result = _service.Convert("USD", "USD", 100);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(100.00m);
        }

        /// <summary>
        /// Verifica que los códigos de moneda sean tratados sin distinguir mayúsculas/minúsculas.
        /// </summary>
        [Fact]
        public void Convert_ShouldBeCaseInsensitive_ForCurrencyCodes()
        {
            var result = _service.Convert("usd", "dop", 1);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(60.50m);
        }

        /// <summary>
        /// Verifica que la conversión falle cuando ambas monedas no están soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenBothCurrenciesUnsupported()
        {
            var result = _service.Convert("BTC", "ETH", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// Verifica varias conversiones usando Theory e InlineData para validar fallback y distintas combinaciones.
        /// </summary>
        [Theory]
        [InlineData("USD", "EUR", 10, 9.30)]
        [InlineData("EUR", "USD", 9.30, 10.00)]
        [InlineData("DOP", "EUR", 128, 2.00)]
        [InlineData("EUR", "DOP", 2, 128.00)]
        public void Convert_ShouldHandleVariousCurrencies_WithFallback(string from, string to, decimal amount, decimal expected)
        {
            var result = _service.Convert(from, to, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Total.Should().Be(expected);
        }
    }
}
