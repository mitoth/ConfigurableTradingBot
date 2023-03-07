using System.Security.Authentication;
using Alpaca.Markets;
using TradingBot.Core.Abstractions;

namespace TradingBot.Core.Services;

public class AlpacaStockPriceService : IStockMarketPricesService
{
    private readonly IAlpacaDataClient _alpacaDataClient;

    public AlpacaStockPriceService(string apiKey, string apiSecret)
    {
        if (string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
            throw new InvalidCredentialException("Please provide api keys for alpaca");
        
        _alpacaDataClient = Environments.Paper
            .GetAlpacaDataClient(new SecretKey(apiKey, apiSecret));
    }

    /// <inheritdoc />
    public async Task<decimal> GetLatestPrice(string symbol, CancellationToken cancellationToken)
    {
        var request = new LatestMarketDataRequest(symbol);
        var quote = await _alpacaDataClient.GetLatestQuoteAsync(request, cancellationToken);
        return quote.AskPrice;
    }
    
    /// <inheritdoc />
    public async Task<decimal> GetHistoricPrice(string symbol, DateTime dateTime, CancellationToken cancellationToken)
    {
        var request = new HistoricalQuotesRequest(symbol, dateTime.AddMinutes(-1), dateTime);
        var quote = await _alpacaDataClient.GetHistoricalQuotesAsync(request, cancellationToken);
        return quote.Items.First().Value.First().AskPrice;
    }
}