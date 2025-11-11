using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class AverageMoneySpentPerProKpi : IKpiCalculator
{
    public string KpiName => "Average Money Spent Per Pro";
    private readonly IMongoCollection<Payment> _payments;

    public AverageMoneySpentPerProKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _payments = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        // Get the number of unique userIds
        var uniqueUserIds = await _payments.Distinct<ObjectId>("userId", FilterDefinition<Payment>.Empty).ToListAsync();
        var uniqueUserIdsCount = uniqueUserIds.Count;
        
        // Aggregate the total sum of all payments
        var pipeline = new[]
        {
            new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", BsonNull.Value },
                        { "totalAmount", new BsonDocument("$sum", "$payment.amountMoney.amount") }
                    }
                }
            }
        };

        var result = await _payments.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
        var totalAmount = result?["totalAmount"].AsInt64 ?? 0;

        // Calculate average spent per user
        var average = uniqueUserIdsCount > 0 ? totalAmount / uniqueUserIdsCount : 0;

        return new KpiResult(KpiName, average);
    }
}