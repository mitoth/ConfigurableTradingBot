using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

/// <summary>
/// Buy stock condition based on other stock's value
/// </summary>
public class OtherStockBasedCondition : OrderCondition
{
    private readonly IStockMarketPricesService _stockMarketPricesService;

    public string? StockName { get; set; }
    public decimal? TargetPriceLowerThan { get; set; }
    public decimal? TargetPriceUpperThan { get; set; }

    public OtherStockBasedCondition(IStockMarketPricesService stockMarketPricesService)
    {
        _stockMarketPricesService = stockMarketPricesService;
    }

    public override string ConditionType { get; set; } 
        = ConditionTypeEnum.OtherStockBasedCondition.ToString();

    public override async Task<bool> IsFulfilled()
    {
        var latestPrice = await _stockMarketPricesService.GetLatestPrice(StockName, new CancellationToken());
        if (TargetPriceUpperThan.HasValue &&
            TargetPriceUpperThan < latestPrice)
        {
            return false;
        }
        if (TargetPriceLowerThan.HasValue &&
            TargetPriceLowerThan > latestPrice)
        {
            return false;
        }

        return true;
    }
}