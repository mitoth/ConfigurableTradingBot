using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

public class NewsBasedCondition : OrderCondition
{
    public string SearchString { get; set; }
    public bool NewsShouldBePositive { get; set; }

    public override string ConditionType { get; }
        = ConditionTypeEnum.NewsBasedCondition.ToString();

    public override bool IsFulfilled()
    {
        return true;
    }
}