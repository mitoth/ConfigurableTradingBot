using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Models;

/// <summary>
/// Buy stock condition based on other stock's value
/// </summary>
public class OtherStockBasedCondition : OrderCondition
{
    private readonly IStockMarketPricesService _stockMarketPricesService;

    public string StockName { get; init; }
    public decimal? TargetPriceLowerThan { get; init; }
    public decimal? TargetPriceUpperThan { get; init; }
    
    public OtherStockBasedCondition(IStockMarketPricesService stockMarketPricesService)
    {
        _stockMarketPricesService = stockMarketPricesService;
    }

    public override bool IsFulfilled()
    {
        var latestPrice = _stockMarketPricesService.GetLatestPrice(StockName);
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