using System.Reflection;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;
using TradingBot.Core.Repository;
using static NUnit.Framework.Assert;

namespace TradingBot.RepositoryTests;

public class RepositoryTests
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockMarketPricesService _marketPricesService;
    private readonly string _testDbName;
    private readonly MongoClient _client;

    public RepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly())
            .AddEnvironmentVariables()
            .Build();
        
        _testDbName = "test";
        _client = new MongoClient(
                configuration.GetValue<string>("COSMOS_CONNECTION_STRING") 
                ?? throw new InvalidOperationException("Cannot find COSMOS_CONNECTION_STRING env variable"));
        var validatorMock = new Mock<IValidator<Order>>();
        validatorMock.Setup(v =>
                v.ValidateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _orderRepository = new OrderRepository(_client, validatorMock.Object, _testDbName);
        _marketPricesService = new Mock<IStockMarketPricesService>().Object;
    }

    [TearDown]
    public void TearDown()
    {
        _client.DropDatabase(_testDbName);
    }
    
    [Test]
    public async Task AddGetUpdateDeleteOrder()
    {
        var id = Guid.NewGuid().ToString();
        var initialOrder = new Order()
        {
            Id = id,
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
                },
                new PriceChangeCondition()
                {
                    Quantity = 6,
                    Symbol = "msft",
                    Increase = -10.5m,
                    BasePrice = 66.8m
                }
            }
        };
        await _orderRepository.CreateAsync(initialOrder);
        var res = await _orderRepository.GetAsync();
        var order = await _orderRepository.GetAsync(id);
        That(res.Any(), Is.True);
        order.Should().BeEquivalentTo(initialOrder);
        var updatedOrder = new Order()
        {
            Id = id,
            Price = 100,
            Quantity = 23,
            Symbol = "MSFT",
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
                },
                new NewsBasedCondition()
                {
                    SearchString = "RVN",
                    NewsShouldBePositive = false
                },
                new PriceChangeCondition()
                {
                    Quantity = 6,
                    Symbol = "msft",
                    Increase = 1.5m,
                    BasePrice = 6.8m
                }
            }
        };
        await _orderRepository.UpdateAsync(updatedOrder);
        order = await _orderRepository.GetAsync(id);
        order.Should().BeEquivalentTo(updatedOrder);
        await _orderRepository.DeleteAsync(id);
        order = await _orderRepository.GetAsync(id);
        Assert.Null(order);
    }
}