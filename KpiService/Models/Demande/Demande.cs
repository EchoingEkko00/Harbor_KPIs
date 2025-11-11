using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models.Demande;

[BsonIgnoreExtraElements]
public class Demande
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    [BsonElement("details")]
    public Details Details { get; set; } = null!;
}