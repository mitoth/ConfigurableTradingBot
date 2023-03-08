using FluentValidation;
using JsonSubTypes;
using MongoDB.Driver;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;
using TradingBot.Core.Models.Validators;
using TradingBot.Core.Repository;
using TradingBot.Core.Services;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetValue("COSMOS_CONNECTION_STRING", "") 
                          ?? throw new InvalidOperationException("Cannot find COSMOS_CONNECTION_STRING env variable");

// Add services to the container.

builder.Services
    .AddControllers().
    AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(
            JsonSubtypesConverterBuilder
                .Of(typeof(OrderCondition), nameof(OrderCondition.ConditionType))
                .RegisterSubtype(typeof(NewsBasedCondition), ConditionTypeEnum.NewsBasedCondition)
                .RegisterSubtype(typeof(OtherStockBasedCondition), ConditionTypeEnum.OtherStockBasedCondition)
                .RegisterSubtype(typeof(PriceChangeCondition), ConditionTypeEnum.PriceChangeCondition)
                .SerializeDiscriminatorProperty()
                .Build()
        );
    });

var apiKey = builder.Configuration.GetValue<string>("ALPACA_API_KEY");
var apiSecret = builder.Configuration.GetValue<string>("ALPACA_API_SECRET");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<NewsBasedCondition>, NewsBasedConditionValidator>();
builder.Services.AddScoped<IValidator<OtherStockBasedCondition>, OtherStockBasedConditionValidator>();
builder.Services.AddScoped<IValidator<Order>, OrderValidator>();
builder.Services.AddSingleton<MongoClient>(_ => new MongoClient(connectionString));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IStockMarketPricesService>(_ 
    => new AlpacaStockPriceService(apiKey, apiSecret));

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

public partial class Program { }
