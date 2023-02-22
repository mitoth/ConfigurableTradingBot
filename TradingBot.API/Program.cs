using JsonSubTypes;
using MongoDB.Driver;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;
using TradingBot.Core.Repository;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetValue("COSMOS_CONNECTION_STRING", "");

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
                .SerializeDiscriminatorProperty()
                .Build()
        );
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoClient>(s => new MongoClient(connectionString));
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

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