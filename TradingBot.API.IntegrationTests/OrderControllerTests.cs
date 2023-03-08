using System.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using TradingBot.API.IntegrationTests.Base;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;

namespace TradingBot.API.IntegrationTests;

[TestFixture]
public class OrderControllerTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly IStockMarketPricesService _marketPricesService;


    public OrderControllerTests()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _marketPricesService = new Mock<IStockMarketPricesService>().Object;
    }
    
    [Test]
    public async Task ReturnsSuccessResult()
    {
        var client = _factory.GetAnonymousClient();

        var response = await client.GetAsync("/api/order/all");

        response.EnsureSuccessStatusCode();
    }
    
    [Test]
    public async Task AddOrderTest()
    {
        var client = _factory.GetAnonymousClient();

        var order = new Order()
        {
            Price = 100,
            Quantity = 23,
            Symbol = "AAPL",
            OrderType = OrderType.Sell,
            BuyOrderConditions = new List<OrderCondition>()
            {
                new OtherStockBasedCondition(_marketPricesService)
                {
                    StockName = "MSFT",
                    TargetPriceLowerThan = 100,
                    TargetPriceUpperThan = 200
                },
                new NewsBasedCondition()
                {
                    SearchString = "fcsb",
                    NewsShouldBePositive = false
                }
            }
        };
        var orderJson = JsonConvert.SerializeObject(order);

        var buffer = System.Text.Encoding.UTF8.GetBytes(orderJson);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await client.PostAsync("/api/order", byteContent);

        response.EnsureSuccessStatusCode();
    }
}