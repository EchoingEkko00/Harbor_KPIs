using KpiService.Models;
using KpiService.Models.Demande;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class PurchaseConversionRateKpi : IKpiCalculator
{
    public string KpiName => "Pro Purchase Conversion Rate";
    private readonly IMongoCollection<User> _users;
    private readonly IMongoCollection<Payment> _payment;

    public PurchaseConversionRateKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _users = mainDb.GetCollection<User>("User");
        _payment = mainDb.GetCollection<Payment>("Payment");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        // Step 1: Count all "Pro" users
        var proFilter = Builders<User>.Filter.Eq(u => u.Type, "Pro");
        var proCount = await _users.CountDocumentsAsync(proFilter);
        
        // Step 2: Get distinct userIds from Payment collection
        var uniquePayerIds = await _payment.Distinct<ObjectId>("userId", FilterDefinition<Payment>.Empty).ToListAsync();
        var uniquePayerCount = uniquePayerIds.Count;
        
        // Step 3: Calculate conversion rate
        double conversionRate = proCount == 0 ? 0 : (double)uniquePayerCount / proCount;

        return new KpiResult(KpiName, conversionRate);
    }
}