using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class TotalCoinsPurchasedKpi : IKpiCalculator
{
    public string KpiName => "Total Coins Purchased";
    private readonly IMongoCollection<Payment> _payments;

    public TotalCoinsPurchasedKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _payments = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var payments = await _payments.Find(FilterDefinition<Payment>.Empty).ToListAsync();

        var totalCoins = payments.Sum(p => p.CoinQty);

        return new KpiResult(KpiName, totalCoins);
    }
}