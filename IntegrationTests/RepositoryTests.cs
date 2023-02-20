using System.Globalization;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;
using TradingBot.Core.Repository;
using static NUnit.Framework.Assert;

namespace IntegrationTests;

public class RepositoryTests
{
    private IOrderRepository _orderRepository;
    private IStockMarketPricesService _marketPricesService;
    private string _testDBName;
    private MongoClient _client;

    [SetUp]
    public void Setup()
    {
        _testDBName = "test";
        _client = new MongoClient(
            Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));
        _orderRepository = new OrderRepository(_client, _testDBName);
        _marketPricesService = new Mock<IStockMarketPricesService>().Object;
    }

    [TearDown]
    public void TearDown()
    {
        _client.DropDatabase(_testDBName);
    }
    
    [Test]
    public async Task AddGetOrder()
    {
        var id = Guid.NewGuid().ToString();
        var order1 = new Order()
        {
            Id = id,
            Price = 100,
            Quantity = 23,
            Symbol = "AAPL",
            OrderType = OrderType.Sell,
            OrderConditions = new List<OrderCondition>()
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
        await _orderRepository.CreateAsync(order1);
        var res = await _orderRepository.GetAsync();
        var order = await _orderRepository.GetAsync(id);
        That(res.Any(), Is.True);
        order.Should().BeEquivalentTo(order1);
    }
}