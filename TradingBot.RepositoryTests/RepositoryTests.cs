using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
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
        _testDbName = "test";
        _client = new MongoClient(
            Environment.GetEnvironmentVariable("COSMOS_CONNECTION_STRING"));
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
                },
                new NewsBasedCondition()
                {
                    SearchString = "RVN",
                    NewsShouldBePositive = false
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