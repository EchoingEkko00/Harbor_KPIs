using KpiService.Models;
using MongoDB.Driver;

namespace KpiService.Repository;

public class MongoKpiRepository : IKpiRepository
{
    private readonly IMongoDatabase _kpiDb;

    public MongoKpiRepository(IConfiguration config)
    {
        var kpiUri = config["MONGO_KPI_URI"];
        var kpiClient = new MongoClient(kpiUri);
        _kpiDb = kpiClient.GetDatabase("KPIs");
    }

    public async Task UpsertKpiAsync(KpiResult result)
    {
        // Use KPI name as the collection name
        var collection = _kpiDb.GetCollection<KpiResult>(result.Name);

        // Just insert a new document every time (not upsert anymore)
        await collection.InsertOneAsync(result);

        Console.WriteLine($"Inserted into '{result.Name}' collection: {result.Value}");
    }
}