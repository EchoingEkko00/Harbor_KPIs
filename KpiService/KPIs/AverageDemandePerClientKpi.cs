using KpiService.Models;
using KpiService.Models.Demande;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KpiService.KPIs;

public class AverageDemandePerClientKpi : IKpiCalculator
{
    public string KpiName => "Average Demande Per Client";
    private readonly IMongoCollection<Demande> _demande;

    public AverageDemandePerClientKpi(IConfiguration config)
    {
        var mainUri = config["MONGO_MAIN_URI"];
        var mainClient = new MongoClient(mainUri);
        var mainDb     = mainClient.GetDatabase("HarborMainDB");
        _demande = mainDb.GetCollection<Demande>("Demande");
    }

    public async Task<KpiResult> CalculateAsync()
    {
        // Total number of demande documents
        var totalDemandes = await _demande.CountDocumentsAsync(FilterDefinition<Demande>.Empty);

        var distinctClientIds = await _demande.DistinctAsync<ObjectId>("details.clientId", FilterDefinition<Demande>.Empty);
        var uniqueClientCount = (await distinctClientIds.ToListAsync()).Count;

        var average = uniqueClientCount > 0 ? (double)totalDemandes / uniqueClientCount : 0;

        return new KpiResult(KpiName, average);
    }
}