using FluentValidation;
using JsonSubTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;
using TradingBot.Core.Models.Validators;
using TradingBot.Core.Repository;

namespace TradingBot.API.IntegrationTests.Base;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<TStartup>()
            .AddEnvironmentVariables()
            .Build();
        
        string connectionString = configuration.GetValue<string>("COSMOS_CONNECTION_STRING") 
                                  ?? throw new InvalidOperationException("Cannot find COSMOS_CONNECTION_STRING env variable");
        builder.ConfigureServices(services =>
        {
            services.AddControllers().
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
            
            services.AddScoped<IValidator<NewsBasedCondition>, NewsBasedConditionValidator>();
            services.AddScoped<IValidator<OtherStockBasedCondition>, OtherStockBasedConditionValidator>();
            services.AddScoped<IValidator<Order>, OrderValidator>();
            services.AddSingleton<MongoClient>(_ => new MongoClient(connectionString));
            services.AddTransient<IOrderRepository, OrderRepository>();
        });
    }
    
    public HttpClient GetAnonymousClient()
    {
        return CreateClient();
    }
}