using KpiService;
using KpiService.KPIs;
using KpiService.Repository;
using Microsoft.Extensions.DependencyInjection;

// Detect run-once mode via CLI flag or environment variable
var runOnce = args.Contains("--run-once") || string.Equals(Environment.GetEnvironmentVariable("RUN_ONCE"), "true", StringComparison.OrdinalIgnoreCase);

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        //User KPIs
        services.AddSingleton<IKpiCalculator, TotalUsersKpi>(); 
        services.AddSingleton<IKpiCalculator, ProCountKpi>();
        services.AddSingleton<IKpiCalculator, ClientCountKpi>();
        services.AddSingleton<IKpiCalculator, AdminCountKpi>();
        services.AddSingleton<IKpiCalculator, NewProsThisTodayKpi>();
        services.AddSingleton<IKpiCalculator, NewClientsTodayKpi>();
        services.AddSingleton<IKpiCalculator, ActiveClientsKpi>();
        services.AddSingleton<IKpiCalculator, ActiveProsKpi>();
        
        //Revenue KPIs
        services.AddSingleton<IKpiCalculator, TotalPurchasesMadeKpi>();
        services.AddSingleton<IKpiCalculator, TotalCoinsPurchasedKpi>();
        services.AddSingleton<IKpiCalculator, TotalRevenueKpi>();
        services.AddSingleton<IKpiCalculator, AverageCoinsBoughtPerProKpi>();
        services.AddSingleton<IKpiCalculator, PurchaseConversionRateKpi>();
        services.AddSingleton<IKpiCalculator, RevenueTodayKpi>();
        services.AddSingleton<IKpiCalculator, AverageMoneySpentPerProKpi>();
        
        
        //Demand KPIs
        services.AddSingleton<IKpiCalculator, AverageDemandePerClientKpi>();
        
        //mongo Repo
        services.AddSingleton<IKpiRepository, MongoKpiRepository>();
        
        //operation-necessary classes
        services.AddSingleton<KpiRunner>();
        services.AddHostedService<Worker>();
    })
    .Build();

if (runOnce)
{
    // Resolve KpiRunner from DI and run once
    using var scope = host.Services.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<KpiRunner>();
    await runner.RunAsync();
    // exit immediately after one run
    return;
}

await host.RunAsync();