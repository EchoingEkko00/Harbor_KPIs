using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models.Demande;

[BsonIgnoreExtraElements]
public class ServiceInfo
{
    [BsonElement("nameEn")]
    public string Service { get; set; } = null!;
}