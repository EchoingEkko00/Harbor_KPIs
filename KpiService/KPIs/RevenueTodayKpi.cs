using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class RevenueTodayKpi : IKpiCalculator
{
    public string KpiName => "Revenue Today";
    private readonly IMongoCollection<Payment> _payments;

    public RevenueTodayKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _payments = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var filterDate = DateTime.UtcNow.AddDays(-1);

        var pipeline = new[]
        {
            new BsonDocument
            {
                { "$match", new BsonDocument("createdAt", new BsonDocument("$gte", filterDate)) }
            },
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

        return new KpiResult(KpiName, totalAmount);
    }
}