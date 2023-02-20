﻿using MongoDB.Driver;
using TradingBot.Core.Abstractions;
using TradingBot.Core.Models;

namespace TradingBot.Core.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _ordersCollection;

    public OrderRepository(MongoClient mongoClient, string dbName = "tradingBot",
        string collectionName = "orders")
    {
        _ordersCollection = mongoClient.GetDatabase(dbName)
            .GetCollection<Order>(collectionName);
    }
    
    public async Task CreateAsync(Order order) =>
         await _ordersCollection.InsertOneAsync(order);
    

    public async Task<Order> GetAsync(string id) =>
        await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();


    public async Task<IEnumerable<Order>> GetAsync() =>
        await _ordersCollection.Find(_ => true).ToListAsync();

    public async Task<bool> UpdateAsync(Order order)
    {
        var updateResult = await _ordersCollection.ReplaceOneAsync(x => x.Id == order.Id,
            order);
        return updateResult.ModifiedCount == 1;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleteResult = await _ordersCollection.DeleteOneAsync(x => x.Id == id);
        return deleteResult.DeletedCount == 1;
    }
}