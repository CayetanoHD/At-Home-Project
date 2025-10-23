using ExchangeApp.ConsoleApp.ExchangeConsole.Runner;
using ExchangeApp.ConsoleApp.ExchangeConsole.Services;
using ExchangeApp.Core.Application.Interfaces;
using ExchangeApp.Core.Application.Interfaces.Console;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;
using ExchangeApp.Core.Application.Services;
using ExchangeApp.Infrastructure.ExternalProviders.ExternalCallService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExchangeApp.Core.Application.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // 🔹 Configurar los HttpClient con sus URLs
        services.AddHttpClient<IApi1ExchangeService, Api1ExchangeService>(c =>
            c.BaseAddress = new Uri("https://localhost:7132/"));

        services.AddHttpClient<IApi2ExchangeService, Api2ExchangeService>(c =>
            c.BaseAddress = new Uri("https://localhost:7140/"));

        services.AddHttpClient<IApi3ExchangeService, Api3ExchangeService>(c =>
            c.BaseAddress = new Uri("https://localhost:7276/"));

        // 🔹 API principal (Parent)
        services.AddHttpClient<IApiParentFallbackService, ApiParentFallbackService>(c =>
            c.BaseAddress = new Uri("https://localhost:7239/api/"));

        services.AddTransient<IBestRateService, BestRateService>();

        services.AddTransient<ConsoleRunner>();

        services.AddLogging(config =>
        {
            config.AddConsole();
            config.SetMinimumLevel(LogLevel.Information);
        });
    })
    .Build();

// 🔸 Ejecutar el runner
using var scope = host.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<ConsoleRunner>();
await runner.RunAsync();
