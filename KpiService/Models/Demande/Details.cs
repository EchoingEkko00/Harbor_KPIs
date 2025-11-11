using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KpiService.Models.Demande;

[BsonIgnoreExtraElements]
public class Details
{
    [BsonElement("clientId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ClientId { get; set; }

    [BsonElement("serviceInfo")]
    public ServiceInfo ServiceInfo { get; set; } = null!;
}