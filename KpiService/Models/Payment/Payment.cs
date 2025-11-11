using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models;
[BsonIgnoreExtraElements]
public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = null!;
    
    [BsonElement("coinsQty")]
    public int CoinQty { get; set; }
    
    [BsonElement("payment")]
    public PaymentDetails PaymentDetail { get; set; } = null!;
}