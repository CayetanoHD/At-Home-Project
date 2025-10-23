using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.Interfaces;
using ExchangeApp.Core.Application.Interfaces.Console;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeApp.ConsoleApp.ExchangeConsole.Services
{
    public class ApiParentFallbackService : IApiParentFallbackService
    {
        private readonly HttpClient _httpClient;
        private readonly IBestRateService _bestRateService;
        private readonly ILogger<ApiParentFallbackService> _logger;

        public ApiParentFallbackService(
            HttpClient httpClient,
            IBestRateService bestRateService,
            ILogger<ApiParentFallbackService> logger)
        {
            _httpClient = httpClient;
            _bestRateService = bestRateService;
            _logger = logger;
        }

        /// <summary>
        /// Servicio que consulta la API principal para obtener la mejor oferta de cambio.
        /// Si la API principal falla o no responde correctamente, utiliza un servicio local como fallback.
        /// </summary>
        public async Task<object> GetBestOfferAsync(ExchangeRequestDto request)
        {
            try
            {
                _logger.LogInformation("Consultando API principal...");
                var response = await _httpClient.PostAsJsonAsync("exchange/best-offer", request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("La API principal no respondió correctamente. Usando fallback local...");
                    return await _bestRateService.GetBestRateAsync(request);
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar API principal. Usando fallback...");
                return await _bestRateService.GetBestRateAsync(request);
            }
        }
    }
}
