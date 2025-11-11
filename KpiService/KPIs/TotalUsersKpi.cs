using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class TotalUsersKpi : IKpiCalculator
{
    public string KpiName => "Total User Count";
    private readonly IMongoCollection<User> _users;

    public TotalUsersKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _users = mainDb.GetCollection<User>("User");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var count = await _users.CountDocumentsAsync(FilterDefinition<User>.Empty);
        return new KpiResult(KpiName, count);
    }
}