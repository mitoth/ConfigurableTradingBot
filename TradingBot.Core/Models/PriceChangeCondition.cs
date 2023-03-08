using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class PriceChangeCondition : OrderCondition
{
    public override string ConditionType { get; set; } 
        = ConditionTypeEnum.PriceChangeCondition.ToString();    
    
    public string? Symbol { get; set; }
    public int Quantity { get; set; }
    public decimal BasePrice { get; set; }
    public decimal Increase { get; set; }
    public override Task<bool> IsFulfilled()
    {
        throw new NotImplementedException();
    }
}