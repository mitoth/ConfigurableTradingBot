using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class Order
{
    public string Id { get; init; }
    public OrderType OrderType { get; init; }
    public string Symbol { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public List<OrderCondition> OrderConditions { get; set; }
}