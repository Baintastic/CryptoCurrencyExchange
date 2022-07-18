using CryptoCurrencyExchange.Core.IServices;
using CryptoCurrencyExchange.Core.Services;
using CryptoCurrencyExchange.Data.ExternalServices;
using CryptoCurrencyExchange.Data.IRepositories;
using CryptoCurrencyExchange.Data.Repositories;
using CryptoCurrencyExchange.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExchangeRateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    options => options.MigrationsAssembly("CryptoCurrencyExchange.Data")));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
builder.Services.AddScoped<ICurrencyExchangeClient, CurrencyExchangeClient>();
builder.Services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<CurrencyExchangeClient>();

var app = builder.Build();
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<ExchangeRateDbContext>();
    context.Database.Migrate();
}

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
