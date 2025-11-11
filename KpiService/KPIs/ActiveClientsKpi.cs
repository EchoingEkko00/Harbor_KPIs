using KpiService.Models;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class ActiveClientsKpi : IKpiCalculator
{
    public string KpiName => "Active Client Count";
    private readonly IMongoCollection<User> _users;

    public ActiveClientsKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _users = mainDb.GetCollection<User>("User");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq(u => u.Type, "Client"),
            Builders<User>.Filter.Gte(u => u.LastSeen, oneMonthAgo)
        );
        var count = await _users.CountDocumentsAsync(filter);
        return new KpiResult(KpiName, count);
    }
}