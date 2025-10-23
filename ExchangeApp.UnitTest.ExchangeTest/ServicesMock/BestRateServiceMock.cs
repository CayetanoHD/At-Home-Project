using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;
using ExchangeApp.Core.Application.Interfaces;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;
using ExchangeApp.Core.Application.Results;
using ExchangeApp.Core.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.UnitTest.ExchangeTest.ServicesMock
{
    public class BestRateServiceTests
    {
        private readonly Mock<IApi1ExchangeService> _api1Mock;
        private readonly Mock<IApi2ExchangeService> _api2Mock;
        private readonly Mock<IApi3ExchangeService> _api3Mock;
        private readonly IBestRateService _service;

        public BestRateServiceTests()
        {
            _api1Mock = new Mock<IApi1ExchangeService>();
            _api2Mock = new Mock<IApi2ExchangeService>();
            _api3Mock = new Mock<IApi3ExchangeService>();

            var loggerMock = new Mock<ILogger<BestRateService>>();
            _service = new BestRateService(_api1Mock.Object, _api2Mock.Object, _api3Mock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task GetBestRateAsync_ShouldReturnBestOffer_WhenAllApisSucceed()
        {
            // Arrange
            var request = new ExchangeRequestDto { From = "USD", To = "DOP", Amount = 10 };

            _api1Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Success(new ExchangeResponseDto { ConvertedAmount = 590m, ProviderName = "API1" }));
            _api2Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Success(new ExchangeResponseDto { ConvertedAmount = 605m, ProviderName = "API2" }));
            _api3Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Success(new ExchangeResponseDto { ConvertedAmount = 585m, ProviderName = "API3" }));

            // Act
            var result = await _service.GetBestRateAsync(request);

            // Assert usando reflexión
            var bestOffer = result.GetType().GetProperty("BestOffer")?.GetValue(result);
            bestOffer.Should().NotBeNull();

            var bestOfferDict = (IDictionary<string, object>)bestOffer!;
            bestOfferDict["Api"].Should().Be("API2");
            bestOfferDict["ProviderName"].Should().Be("API2");
            bestOfferDict["ConvertedAmount"].Should().Be(605m);
        }

        [Fact]
        public async Task GetBestRateAsync_ShouldHandlePartialFailures()
        {
            // Arrange
            var request = new ExchangeRequestDto { From = "USD", To = "DOP", Amount = 10 };

            _api1Mock.Setup(a => a.GetRateAsync(It.IsAny<ExchangeRequestDto>()))
                .ReturnsAsync(Result<ExchangeResponseDto>.Failure("API1 error"));
            _api2Mock.Setup(a => a.GetRateAsync(It.IsAny<ExchangeRequestDto>()))
                .ReturnsAsync(Result<ExchangeResponseDto>.Success(new ExchangeResponseDto { ConvertedAmount = 605m, ProviderName = "API2" }));
            _api3Mock.Setup(a => a.GetRateAsync(It.IsAny<ExchangeRequestDto>()))
                .ReturnsAsync(Result<ExchangeResponseDto>.Failure("API3 error"));

            // Act
            var result = await _service.GetBestRateAsync(request);

            // Assert usando reflexión
            var bestOffer = result.GetType().GetProperty("BestOffer")?.GetValue(result);
            bestOffer.Should().NotBeNull();
            var bestOfferDict = (IDictionary<string, object>)bestOffer!;
            bestOfferDict["Api"].Should().Be("API2");
            bestOfferDict["ProviderName"].Should().Be("API2");
            bestOfferDict["ConvertedAmount"].Should().Be(605m);

            var offers = (IEnumerable<object>)result.GetType().GetProperty("Offers")!.GetValue(result)!;
            ((IDictionary<string, object>)offers.ElementAt(0))["Error"].Should().Be("API1 error");
            ((IDictionary<string, object>)offers.ElementAt(1))["ConvertedAmount"].Should().Be(605m);
            ((IDictionary<string, object>)offers.ElementAt(2))["Error"].Should().Be("API3 error");
        }

        [Fact]
        public async Task GetBestRateAsync_ShouldReturnNullBestOffer_WhenAllApisFail()
        {
            // Arrange
            var request = new ExchangeRequestDto { From = "USD", To = "DOP", Amount = 10 };

            _api1Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Failure("API1 error"));
            _api2Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Failure("API2 error"));
            _api3Mock.Setup(a => a.GetRateAsync(request))
                .ReturnsAsync(Result<ExchangeResponseDto>.Failure("API3 error"));

            // Act
            var result = await _service.GetBestRateAsync(request);

            // Assert usando reflexión
            var bestOffer = result.GetType().GetProperty("BestOffer")?.GetValue(result);
            bestOffer.Should().BeNull();

            var offers = (IEnumerable<object>)result.GetType().GetProperty("Offers")!.GetValue(result)!;
            ((IDictionary<string, object>)offers.ElementAt(0))["Error"].Should().Be("API1 error");
            ((IDictionary<string, object>)offers.ElementAt(1))["Error"].Should().Be("API2 error");
            ((IDictionary<string, object>)offers.ElementAt(2))["Error"].Should().Be("API3 error");
        }
    }

}
