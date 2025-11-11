using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class AverageCoinsBoughtPerProKpi : IKpiCalculator
{
    public string KpiName => "Average Coins Bought Per Pro";
    private readonly IMongoCollection<Payment> _payments;

    public AverageCoinsBoughtPerProKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _payments = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        // 1. Sum of all coinsQty in the collection
        var totalCoins = await _payments.Aggregate()
            .Group(new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "totalCoins", new BsonDocument("$sum", "$coinsQty") }
            })
            .FirstOrDefaultAsync();

        int totalCoinSum = totalCoins != null ? totalCoins["totalCoins"].AsInt32 : 0;

        // 2. Get the count of unique userIds
        var distinctUserIds = await _payments.DistinctAsync<ObjectId>("userId", FilterDefinition<Payment>.Empty);
        var uniqueUserCount = (await distinctUserIds.ToListAsync()).Count;

        // 3. Calculate the average (avoid divide-by-zero)
        var average = uniqueUserCount > 0 ? (double)totalCoinSum / uniqueUserCount : 0;

        return new KpiResult(KpiName, average);
    }
}