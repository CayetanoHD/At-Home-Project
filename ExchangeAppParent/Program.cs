using ExchangeApp.Core.Application.Interfaces;
using ExchangeApp.Infrastructure.ExternalProviders.ExternalCallService;
using ExchangeApp.Core.Application.Services;
using ExchangeApp.Core.Application.Interfaces.ExternalApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
       .AddXmlSerializerFormatters(); 

builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true; // Respeta el header Accept del cliente
})
.AddXmlSerializerFormatters() // permite XML
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IApi1ExchangeService, Api1ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:7132/"));
builder.Services.AddHttpClient<IApi2ExchangeService, Api2ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:7140/"));
builder.Services.AddHttpClient<IApi3ExchangeService, Api3ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:7276/"));
builder.Services.AddScoped<IBestRateService, BestRateService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
