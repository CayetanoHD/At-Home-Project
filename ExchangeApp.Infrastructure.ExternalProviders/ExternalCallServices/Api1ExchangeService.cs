using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;
using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;
using ExchangeApp.Core.Application.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Infrastructure.ExternalProviders.ExternalCallService
{
    public class Api1ExchangeService : IApi1ExchangeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<Api1ExchangeService> _logger;

        public Api1ExchangeService(HttpClient httpClient, ILogger<Api1ExchangeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la mejor tasa de cambio para una conversión de moneda usando la API1.
        /// </summary>
        /// <param name="request">
        /// Objeto que contiene los datos de la conversión, incluyendo:
        /// - From: moneda de origen
        /// - To: moneda de destino (solo USD, DOP o EUR)
        /// - Amount: cantidad a convertir
        /// </param>
        /// <returns>
        /// Un objeto <see cref="Result{ExchangeResponseDto}"/> que contiene:
        /// - ConvertedAmount: monto convertido
        /// - ProviderName: nombre del proveedor (API1)
        /// 
        /// Si ocurre un error, el resultado contendrá la descripción del fallo.
        /// </returns>
        public async Task<Result<ExchangeResponseDto>> GetRateAsync(ExchangeRequestDto request)
        {
            try
            {
                // 🔹 Validar monedas permitidas
                var allowedCurrencies = new[] { "USD", "DOP", "EUR" };

                if (string.IsNullOrWhiteSpace(request.To))
                {
                    _logger.LogWarning("Target currency is null or empty");
                    return Result<ExchangeResponseDto>.Failure("Target currency is required.");
                }

                var toCurrency = request.To.ToUpper();
                if (!allowedCurrencies.Contains(toCurrency))
                {
                    _logger.LogWarning("Conversion attempt to unsupported currency: {To}", toCurrency);
                    return Result<ExchangeResponseDto>.Failure(
                       "Only conversions to USD, DOP, or EUR are allowed."
                    );
                }

                _logger.LogInformation("Sending conversion request to API1: {From} -> {To}, amount {Amount}",
                    request.From, toCurrency, request.Amount);

                var response = await _httpClient.PostAsJsonAsync("api/api1/convert", new
                {
                    From = request.From,
                    To = toCurrency,
                    Value = request.Amount
                });

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API1 returned status code {StatusCode}", response.StatusCode);
                    return Result<ExchangeResponseDto>.Failure($"API1 returned status code {response.StatusCode}");
                }

                var data = await response.Content.ReadFromJsonAsync<Api1Response>();
                if (data == null)
                {
                    _logger.LogWarning("API1 returned null response");
                    return Result<ExchangeResponseDto>.Failure("API1 returned null response");
                }

                var resultDto = new ExchangeResponseDto
                {
                    ProviderName = "API1",
                    ConvertedAmount = data.ConvertedAmount
                };

                _logger.LogInformation("API1 conversion result: {ConvertedAmount}", resultDto.ConvertedAmount);

                return Result<ExchangeResponseDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error calling API1 conversion");
                return Result<ExchangeResponseDto>.Failure($"Exception: {ex.Message}");
            }
        }
    }



}

