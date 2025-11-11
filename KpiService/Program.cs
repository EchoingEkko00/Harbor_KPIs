using KpiService;
using KpiService.KPIs;
using KpiService.Repository;

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

await host.RunAsync();