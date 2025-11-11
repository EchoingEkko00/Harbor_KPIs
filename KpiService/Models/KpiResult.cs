using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models;

public class KpiResult
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [BsonElement("value")]
    public object Value { get; set; } = null!;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = null!;

    public KpiResult(string name, object value)
    {
        Name = name;
        Value = value;
        UpdatedAt = DateTime.UtcNow;
    }
}