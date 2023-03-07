using Microsoft.AspNetCore.Mvc;
using TradingBot.Core.Abstractions;

namespace TradingBot.API.Controllers
{
    /// <summary>
    /// APIs for stock market data
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StockPriceController : ControllerBase
    {
        private IStockMarketPricesService _stockMarketPricesService;


        public StockPriceController(IStockMarketPricesService stockMarketPricesService)
        {
            _stockMarketPricesService = stockMarketPricesService;
        }

        /// <summary>
        /// Get the latest price for the specified stock
        /// </summary>
        /// <param name="symbol">The stock symbol</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The latest stock price for the specified symbol</returns>
        [HttpGet("price/{symbol}")]
        public async Task<decimal> Get(string symbol, CancellationToken cancellationToken)
        {
            return await _stockMarketPricesService.GetLatestPrice(symbol, cancellationToken);
        }

        /// <summary>
        /// Get the historical prices for the specified stock
        /// </summary>
        /// <param name="symbol">The stock symbol</param>
        /// <param name="dateTime">The date for which you want to get the stock price</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The latest stock price for the specified symbol</returns>
        [HttpGet("historyPrice/{symbol}")]
        public async Task<decimal> GetHistory(string symbol, DateTime dateTime, CancellationToken cancellationToken)
        {
            return await _stockMarketPricesService.GetHistoricPrice(symbol, dateTime, cancellationToken);

        }
    }
}