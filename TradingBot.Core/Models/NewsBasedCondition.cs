using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class NewsBasedCondition : OrderCondition
{
    public string? SearchString { get; set; }
    public bool NewsShouldBePositive { get; set; }

    public override string ConditionType { get; set; }
        = ConditionTypeEnum.NewsBasedCondition.ToString();

    public override Task<bool> IsFulfilled()
    {
        return Task.FromResult(true);
    }
}