using MongoDB.Bson.Serialization.Attributes;
using TradingBot.Core.Models;

namespace TradingBot.Core.Abstractions;

[BsonKnownTypes(typeof(OtherStockBasedCondition))]
[BsonKnownTypes(typeof(NewsBasedCondition))]
public abstract class OrderCondition
{
    public abstract string ConditionType { get; set; }
    public abstract Task<bool> IsFulfilled();
}