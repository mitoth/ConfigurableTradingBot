namespace TradingBot.Core.Abstractions;

public interface IStockMarketPricesService
{
    /// <summary>
    /// Get a stock Ask price from the past
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="dateTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<decimal> GetHistoricPrice(string symbol, DateTime dateTime, CancellationToken cancellationToken);

    /// <summary>
    /// Get the latest Ask price for the given symbol
    /// </summary>
    /// <param name="symbol"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<decimal> GetLatestPrice(string symbol, CancellationToken cancellationToken);
}