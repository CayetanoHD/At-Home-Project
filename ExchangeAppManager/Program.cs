using ExchangeApp.Core.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Registrar servicios HttpClient
builder.Services.AddHttpClient<Api1ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:5001/"));
builder.Services.AddHttpClient<Api2ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:5002/"));
builder.Services.AddHttpClient<Api3ExchangeService>(c => c.BaseAddress = new Uri("https://localhost:5003/"));

builder.Services.AddScoped<BestRateService>();


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
