using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class Order
{
    public string? Id { get; set; }
    public OrderType OrderType { get; set; }
    public string? Symbol { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public List<OrderCondition> BuyOrderConditions { get; set; } = new List<OrderCondition>();

    public List<OrderCondition> SellOrderConditions { get; set; } = new List<OrderCondition>();

}