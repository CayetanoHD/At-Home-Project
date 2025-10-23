using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.Services.Api1
{
    public class Api1Service : IApi1Service
    {
        private readonly ILogger<Api1Service> _logger;

        public Api1Service(ILogger<Api1Service> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Convierte un monto de una moneda a otra usando tasas simuladas de la API1.
        /// El método soporta solo USD, DOP y EUR, y las tasas son fijas simuladas.
        /// </summary>
        /// <param name="from">Moneda de origen (USD, DOP o EUR).</param>
        /// <param name="to">Moneda de destino (USD, DOP o EUR).</param>
        /// <param name="amount">Cantidad a convertir.</param>
        /// <returns>
        /// Un objeto <see cref="Result{Api1Response}"/> que contiene:
        /// - ConvertedAmount: monto convertido redondeado a 2 decimales.
        /// 
        /// Si ocurre un error o la conversión no está soportada, el resultado contendrá la descripción del fallo.
        /// </returns>

        public Result<Api1Response> Convert(string from, string to, decimal amount)
        {
            try
            {
                from = from.ToUpper();
                to = to.ToUpper();
                _logger.LogInformation($"Converting {amount} from {from} to {to}");

                var allowed = new[] { "USD", "DOP", "EUR" };

                if (!allowed.Contains(from) || !allowed.Contains(to))
                {
                    _logger.LogWarning("Conversion attempt with unsupported currencies: {From} to {To}", from, to);
                    return Result<Api1Response>.Failure("Only USD, DOP, and EUR are supported.");
                }
          
                // Si las monedas son iguales → no hay conversión
                if (from == to)
                {
                    _logger.LogInformation("From and To currencies are the same. Returning original amount.");
                    return Result<Api1Response>.Success(new Api1Response
                    {
                        ConvertedAmount = amount
                    });
                }

                decimal result;

                // 🔹 Tasas fijas simuladas para API1
                const decimal usdToDop = 59.00m;
                const decimal eurToDop = 63.50m;
                const decimal usdToEur = 0.92m;

                // 🔹 Conversión según combinación
                if (from == "USD" && to == "DOP")
                    result = amount * usdToDop;
                else if (from == "DOP" && to == "USD")
                    result = amount / usdToDop;
                else if (from == "EUR" && to == "DOP")
                    result = amount * eurToDop;
                else if (from == "DOP" && to == "EUR")
                    result = amount / eurToDop;
                else if (from == "USD" && to == "EUR")
                    result = amount * usdToEur;
                else if (from == "EUR" && to == "USD")
                    result = amount / usdToEur;
                else 
                {
                    _logger.LogWarning($"Unsupported conversion from {from} to {to}");
                    return Result<Api1Response>.Failure($"Conversion from {from} to {to} is not supported.");
                }

                var converted = Math.Round(result, 2);
                _logger.LogInformation("Conversion result: {ConvertedAmount}", converted);

                return Result<Api1Response>.Success(new Api1Response
                {
                    ConvertedAmount = converted
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during conversion from {From} to {To}", from, to);
                return Result<Api1Response>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }

}
