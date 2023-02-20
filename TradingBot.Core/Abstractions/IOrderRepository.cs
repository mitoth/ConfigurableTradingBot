using TradingBot.Core.Models;

namespace TradingBot.Core.Abstractions;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<Order> GetAsync(string id);
    Task<IEnumerable<Order>> GetAsync();
    Task<bool> UpdateAsync(Order order);
    Task<bool> DeleteAsync(string id);
}