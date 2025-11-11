using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class AdminCountKpi : IKpiCalculator
{
    public string KpiName => "Admin Count";
    private readonly IMongoCollection<User> _users;

    public AdminCountKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _users = mainDb.GetCollection<User>("User");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var filter = Builders<User>.Filter.Eq(u => u.Type, "Admin");
        var count = await _users.CountDocumentsAsync(filter);
        return new KpiResult(KpiName, count);
    }
}