using KpiService.KPIs;
using KpiService.Repository;

namespace KpiService;

public class KpiRunner
{
    private readonly IEnumerable<IKpiCalculator> _calculators;
    private readonly IKpiRepository _kpiRepo;

    public KpiRunner(IEnumerable<IKpiCalculator> calculators, IKpiRepository repo)
    {
        _calculators = calculators;
        _kpiRepo = repo;
    }

    public async Task RunAsync()
    {
        foreach (var calculator in _calculators)
        {
            var result = await calculator.CalculateAsync();
            await _kpiRepo.UpsertKpiAsync(result);
        }
    }
}