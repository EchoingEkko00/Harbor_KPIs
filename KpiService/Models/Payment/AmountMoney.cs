using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models;

[BsonIgnoreExtraElements]
public class AmountMoney
{
    [BsonElement("amount")]
    public long Amount { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = null!;
}