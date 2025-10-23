using ExchangeApp.Test.ExchangeTest.Services.Mocks;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.UnitTest.ExchangeTest.ServicesMock
{
    public class Api1ServiceTests
    {
        /// <summary>
        /// Verifica que la conversión de USD a DOP devuelva el monto convertido correctamente.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenUsdToDop()
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert("USD", "DOP", 10);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(590.00m);
        }

        /// <summary>
        /// Verifica que la conversión falle cuando se usan monedas no soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenCurrencyNotSupported()
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert("BTC", "DOP", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

        /// <summary>
        /// Verifica que la conversión de DOP a USD devuelva el monto convertido correctamente.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenDopToUsd()
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert("DOP", "USD", 590);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(10.00m);
        }

        /// <summary>
        /// Verifica que si la moneda de origen y destino son iguales, se devuelva el mismo monto.
        /// </summary>
        [Fact]
        public void Convert_ShouldReturnSameAmount_WhenFromAndToAreEqual()
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert("USD", "USD", 100);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(100.00m);
        }

        /// <summary>
        /// Verifica distintas conversiones usando fallback cuando no hay tasa simulada.
        /// Incluye mayúsculas/minúsculas y monedas válidas sin tasa definida.
        /// </summary>
        [Theory]
        [InlineData("usd", "dop", 5, 295.00)]
        [InlineData("EUR", "DOP", 2, 127.00)] // fallback default, no tasa simulada
        [InlineData("DOP", "EUR", 10, 0.16)] // fallback default
        public void Convert_ShouldHandleVariousCurrencies_WithFallback(string from, string to, decimal amount, decimal expected)
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert(from, to, amount);

            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(expected);
        }

        /// <summary>
        /// Verifica que la conversión falle cuando ambas monedas no están soportadas.
        /// </summary>
        [Fact]
        public void Convert_ShouldFail_WhenBothCurrenciesUnsupported()
        {
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            var result = service.Convert("BTC", "ETH", 10);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }

      
    }

}
