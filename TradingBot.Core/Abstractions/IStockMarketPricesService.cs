namespace TradingBot.Core.Abstractions;

public interface IStockMarketPricesService
{
    /// <summary>
    /// Get the latest trading price
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    decimal GetLatestPrice(string symbol);
}