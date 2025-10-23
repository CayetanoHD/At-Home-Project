using ExchangeApp.Test.ExchangeTest.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace ExchangeApp.Test.ExchangeTest.Services
{
    public class Api1ServiceTests
    {
        [Fact]
        public void Convert_ShouldReturnConvertedAmount_WhenUsdToDop()
        {
            // Arrange
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            // Act
            var result = service.Convert("USD", "DOP", 10);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.ConvertedAmount.Should().Be(590.00m);
        }

        [Fact]
        public void Convert_ShouldFail_WhenCurrencyNotSupported()
        {
            // Arrange
            var mock = Api1ServiceMock.GetMock();
            var service = mock.Object;

            // Act
            var result = service.Convert("BTC", "DOP", 10);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Only USD, DOP, and EUR are supported.");
        }
    }
}
