using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;
using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;
using ExchangeApp.Core.Application.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.Services
{
    public class BestRateService : IBestRateService
    {
        private readonly IApi1ExchangeService _api1;
        private readonly IApi2ExchangeService _api2;
        private readonly IApi3ExchangeService _api3;
        private readonly ILogger<BestRateService> _logger;

        public BestRateService(
            IApi1ExchangeService api1,
            IApi2ExchangeService api2,
            IApi3ExchangeService api3,
            ILogger<BestRateService> logger)
        {
            _api1 = api1;
            _api2 = api2;
            _api3 = api3;
            _logger = logger;
        }

        /// <summary>
        /// Servicio que consulta múltiples proveedores de cambio de moneda (API1, API2, API3)
        /// y obtiene la mejor oferta según el monto convertido.
        /// </summary>
        public async Task<object> GetBestRateAsync(ExchangeRequestDto request)
        {
            var tasks = new[]
            {
              SafeCall("API1", _api1.GetRateAsync, request),
              SafeCall("API2", _api2.GetRateAsync, request),
              SafeCall("API3", _api3.GetRateAsync, request)
           };

            var results = await Task.WhenAll(tasks);

            // Filtramos solo los resultados exitosos
            var validResults = results.Where(r => r.Response.IsSuccess).ToList();

            // Obtenemos la mejor oferta según ConvertedAmount
            var best = validResults
                .OrderByDescending(r => r.Response.Value!.ConvertedAmount)
                .FirstOrDefault();

            var bestOffer = best == null ? null : new Dictionary<string, object?>
            {
                { "Api", best.Source },
                { "ProviderName", best.Response.Value!.ProviderName },
                { "ConvertedAmount", best.Response.Value!.ConvertedAmount },
                { "ResponseTime", best.ResponseTime.TotalMilliseconds }
            };

            if (best?.Response.IsFailure == true)
            {
                bestOffer!["Error"] = best.Response.Error;
            }

            var offers = results.Select(v =>
            {
                var dict = new Dictionary<string, object?>
                {
                    { "Api", v.Source },
                    { "ProviderName", v.Response.Value?.ProviderName },
                    { "ConvertedAmount", v.Response.Value?.ConvertedAmount },
                    { "ResponseTime", v.ResponseTime.TotalMilliseconds }
                };

                if (v.Response.IsFailure)
                    dict["Error"] = v.Response.Error;

                return dict;
            });

            return new { BestOffer = bestOffer, Offers = offers };

        }

        /// <summary>
        /// Ejecuta una llamada segura a un servicio de conversión de moneda,
        /// registrando tiempo de respuesta y capturando cualquier excepción.
        /// </summary>
        /// <param name="apiName">Nombre del proveedor/API.</param>
        /// <param name="apiCall">Función que realiza la llamada al servicio.</param>
        /// <param name="request">Datos de la conversión.</param>
        /// <returns>Resultado de la llamada incluyendo tiempo de respuesta y error si ocurre.</returns>
        private async Task<ApiCallResult> SafeCall(
            string apiName,
            Func<ExchangeRequestDto, Task<Result<ExchangeResponseDto>>> apiCall,
            ExchangeRequestDto request)
        {
            var sw = Stopwatch.StartNew();
            Result<ExchangeResponseDto> response;

            try
            {
                response = await apiCall(request);
            }
            catch (Exception ex)
            {
                response = Result<ExchangeResponseDto>.Failure(ex.Message);
                _logger.LogError(ex, $"Error calling {apiName}");
            }
            finally
            {
                sw.Stop();
            }

            return new ApiCallResult
            {
                Source = apiName,
                Response = response,
                ResponseTime = sw.Elapsed,
                Error = response.IsFailure ? response.Error : null
            };
        }

    }
}
