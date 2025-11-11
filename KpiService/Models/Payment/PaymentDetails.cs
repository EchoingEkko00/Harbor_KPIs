using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models;

[BsonIgnoreExtraElements]
public class PaymentDetails
{
    [BsonElement("amountMoney")]
    public AmountMoney AmountMoney { get; set; } = null!;
}