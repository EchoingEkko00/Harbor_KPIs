using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models;
[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    [BsonElement("type")]
    public string? Type { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("active")]
    public bool Active { get; set; }
    
    [BsonElement("lastSeen")]
    public DateTime LastSeen { get; set; }
}