using ExchangeApp.Core.Application.DTOS.Apis.Api2;
using ExchangeApp.Core.Application.Interfaces.Apis;
using ExchangeApp.Core.Application.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.Services.Api2
{
    public class Api2Service : IApi2Service
    {
        private readonly ILogger<Api2Service> _logger;

        public Api2Service(ILogger<Api2Service> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Convierte un monto de una moneda a otra usando tasas simuladas de la API2.
        /// El método soporta solo USD, DOP y EUR, y el dólar tiene la tasa más cara que en API1.
        /// </summary>
        /// <param name="from">Moneda de origen (USD, DOP o EUR).</param>
        /// <param name="to">Moneda de destino (USD, DOP o EUR).</param>
        /// <param name="amount">Cantidad a convertir.</param>
        /// <returns>
        /// Un objeto <see cref="Result{Api2Response}"/> que contiene:
        /// - Total: monto convertido redondeado a 2 decimales.
        /// 
        /// Si ocurre un error o la conversión no está soportada, el resultado contendrá la descripción del fallo.
        /// </returns>
        public Result<Api2Response> Convert(string from, string to, decimal amount)
        {
            try
            {
                from = from.ToUpper();
                to = to.ToUpper();

                _logger.LogInformation("Converting {Amount} from {From} to {To}", amount, from, to);

                var allowed = new[] { "USD", "DOP", "EUR" };
                if (!allowed.Contains(from) || !allowed.Contains(to))
                {
                    _logger.LogWarning("Conversion attempt with unsupported currencies: {From} to {To}", from, to);
                    return Result<Api2Response>.Failure("Only USD, DOP, and EUR are supported.");
                }

                // Si las monedas son iguales → no hay conversión
                if (from == to)
                {
                    _logger.LogInformation("From and To currencies are the same. Returning original amount.");
                    return Result<Api2Response>.Success(new Api2Response { Total = amount });
                }

                decimal result;

                // 🔹 Tasas simuladas para API2 (dólar más caro que API1)
                const decimal usdToDop = 60.50m;
                const decimal eurToDop = 64.00m;
                const decimal usdToEur = 0.93m;

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
                    _logger.LogWarning("Unsupported conversion from {From} to {To}", from, to);
                    return Result<Api2Response>.Failure($"Conversion from {from} to {to} is not supported.");
                }

                var converted = Math.Round(result, 2);
                _logger.LogInformation("Conversion result: {ConvertedAmount}", converted);

                return Result<Api2Response>.Success(new Api2Response { Total = converted });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during conversion from {From} to {To}", from, to);
                return Result<Api2Response>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }


}
