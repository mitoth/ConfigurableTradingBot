using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class NewsBasedCondition : OrderCondition
{
    public string SearchString { get; init; }
    public bool NewsShouldBePositive { get; init; }
    
    public override bool IsFulfilled()
    {
        return true;
    }
}