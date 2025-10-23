using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.DTOS;
using ExchangeApp.Core.Application.DTOS.Apis;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;
using ExchangeApp.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeApp.Infrastructure.ExternalProviders.ExternalCallService
{

    public class Api3ExchangeService : IApi3ExchangeService
    {
        private readonly HttpClient _httpClient;

        public Api3ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtiene la tasa de cambio para una conversión de moneda usando la API3.
        /// La solicitud se envía en formato XML y se recibe una respuesta en XML.
        /// </summary>
        /// <param name="request">
        /// Objeto que contiene los datos de la conversión:
        /// - From: moneda de origen
        /// - To: moneda de destino (solo USD, DOP o EUR)
        /// - Amount: cantidad a convertir
        /// </param>
        /// <returns>
        /// Un objeto <see cref="Result{ExchangeResponseDto}"/> que contiene:
        /// - ConvertedAmount: monto convertido según la API3
        /// - ProviderName: nombre del proveedor ("API3")
        /// 
        /// En caso de error, el resultado contendrá la descripción del fallo.
        /// </returns>
        public async Task<Result<ExchangeResponseDto>> GetRateAsync(ExchangeRequestDto request)
        {
            try
            {
                // 🔹 Validate allowed target currencies
                var allowedCurrencies = new[] { "USD", "DOP", "EUR" };

                if (!allowedCurrencies.Contains(request.To.ToUpper()))
                {
                    return Result<ExchangeResponseDto>.Failure(
                        "Only conversions to USD, DOP, or EUR are allowed."
                    );
                }

                // 1️⃣ Prepare XML request
                var xmlRequest = new Api3Request
                {
                    From = request.From,
                    To = request.To,
                    Amount = request.Amount
                };

                var serializer = new XmlSerializer(typeof(Api3Request));

                // Remove xmlns:xsi and xmlns:xsd
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                string xmlString;
                using (var ms = new MemoryStream())
                {
                    var settings = new XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        OmitXmlDeclaration = false,
                        Indent = true
                    };

                    using (var writer = XmlWriter.Create(ms, settings))
                    {
                        serializer.Serialize(writer, xmlRequest, namespaces);
                    }

                    xmlString = Encoding.UTF8.GetString(ms.ToArray());
                }

                // Logging request
                Console.WriteLine("XML sent to API3:");
                Console.WriteLine(xmlString);

                var content = new StringContent(xmlString, Encoding.UTF8, "application/xml");

                // 2️⃣ Send request to API3
                var response = await _httpClient.PostAsync("api/api3/convert", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Logging response
                Console.WriteLine("StatusCode API3: " + response.StatusCode);
                Console.WriteLine("Response from API3:");
                Console.WriteLine(responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = $"API3 returned status code {response.StatusCode}";
                    Console.WriteLine(errorMsg);
                    return Result<ExchangeResponseDto>.Failure(errorMsg);
                }

                // 3️⃣ Deserialize XML response
                var responseSerializer = new XmlSerializer(typeof(Api3Response));
                using var reader = new StringReader(responseContent);
                var data = (Api3Response?)responseSerializer.Deserialize(reader);

                if (data == null)
                {
                    var errorMsg = "API3 returned null response after deserialization";
                    Console.WriteLine(errorMsg);
                    return Result<ExchangeResponseDto>.Failure(errorMsg);
                }

                // 4️⃣ Convert to DTO
                var resultDto = new ExchangeResponseDto
                {
                    ProviderName = "API3",
                    ConvertedAmount = data.Result
                };

                return Result<ExchangeResponseDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                var errorMsg = $"Exception in API3 GetRateAsync: {ex.Message}";
                Console.WriteLine(errorMsg);
                return Result<ExchangeResponseDto>.Failure(errorMsg);
            }
        }
    }
}
