using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class TotalPurchasesMadeKpi : IKpiCalculator
{
    public string KpiName => "Total Purchases Made";
    private readonly IMongoCollection<Payment> _payments;

    public TotalPurchasesMadeKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _payments = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var count = await _payments.CountDocumentsAsync(FilterDefinition<Payment>.Empty);
        return new KpiResult(KpiName, count);
    }
}