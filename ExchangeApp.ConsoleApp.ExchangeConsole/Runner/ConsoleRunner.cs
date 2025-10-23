using ExchangeApp.ConsoleApp.ExchangeConsole.Services;
using ExchangeApp.Core.Application.Dtos;
using ExchangeApp.Core.Application.Interfaces.Console;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeApp.ConsoleApp.ExchangeConsole.Runner
{

    public class ConsoleRunner
    {
        private readonly IApiParentFallbackService _apiParent;
        private readonly HashSet<string> _allowedCurrencies = new() { "USD", "DOP", "EUR" };

        public ConsoleRunner(IApiParentFallbackService apiParent)
        {
            _apiParent = apiParent;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("===============================================");
            Console.WriteLine(" Welcome to the Currency Conversion System");
            Console.WriteLine("===============================================\n");

            while (true)
            {
                Console.Write("Enter source currency (USD, DOP, EUR): ");
                var from = Console.ReadLine()?.ToUpper();

                Console.Write("Enter target currency (USD, DOP, EUR): ");
                var to = Console.ReadLine()?.ToUpper();

                // Validate allowed currencies
                if (!_allowedCurrencies.Contains(from) || !_allowedCurrencies.Contains(to))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n⚠ Only USD, DOP, and EUR can be converted.\n");
                    Console.ResetColor();
                    continue;
                }

                if (from == to)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n⚠ Cannot convert from {from} to {to}, they are the same currency.\n");
                    Console.ResetColor();
                    continue;
                }

                Console.Write("Enter amount to convert: ");
                if (!decimal.TryParse(Console.ReadLine(), out var amount) || amount <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n⚠ The entered amount is not valid.\n");
                    Console.ResetColor();
                    continue;
                }

                var request = new ExchangeRequestDto { From = from, To = to, Amount = amount };

                Console.WriteLine("\nProcessing conversion...\n");

                var result = await _apiParent.GetBestOfferAsync(request);

                if (result is null)
                {
                    Console.WriteLine("Could not obtain any offer.");
                }
                else
                {
                    // Extract BestOffer and Offers dynamically
                    var jsonElement = JsonSerializer.SerializeToElement(result);

                    var bestOffer = jsonElement.GetProperty("BestOffer");
                    var offers = jsonElement.GetProperty("Offers");

                    // 🎯 Best offer
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("🎯 Best Offer:");
                    Console.ResetColor();
                    Console.WriteLine($"  API: {bestOffer.GetProperty("Api").GetString()}");
                    Console.WriteLine($"  Provider: {bestOffer.GetProperty("ProviderName").GetString()}");
                    Console.WriteLine($"  Converted Amount: {bestOffer.GetProperty("ConvertedAmount").GetDecimal():N2}");
                    Console.WriteLine($"  Response Time: {bestOffer.GetProperty("ResponseTime").GetDouble():N2} ms");
                    if (bestOffer.TryGetProperty("Error", out var errorProp))
                        Console.WriteLine($"  ⚠ Error: {errorProp.GetString()}");

                    // 💹 All offers
                    Console.WriteLine("\n💹 All Offers:");
                    Console.WriteLine($"{"API",-5} {"Provider",-10} {"Converted Amount",-18} {"Time(ms)",-12} {"Error",-30}");
                    Console.WriteLine(new string('-', 80));

                    foreach (var offer in offers.EnumerateArray())
                    {
                        var api = offer.GetProperty("Api").GetString();
                        var provider = offer.GetProperty("ProviderName").GetString() ?? "-";
                        var converted = offer.GetProperty("ConvertedAmount").ValueKind != JsonValueKind.Null
                                        ? offer.GetProperty("ConvertedAmount").GetDecimal().ToString("N2")
                                        : "-";
                        var time = offer.GetProperty("ResponseTime").GetDouble().ToString("N2");
                        var error = offer.TryGetProperty("Error", out var err) ? err.GetString() ?? "-" : "-";

                        Console.WriteLine($"{api,-5} {provider,-10} {converted,-18} {time,-12} {error,-30}");
                    }
                }

                Console.WriteLine("\nDo you want to make another conversion? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                    break;

                Console.Clear();
            }

            Console.WriteLine("Thank you for using the system. Goodbye!");
        }
    }


}

