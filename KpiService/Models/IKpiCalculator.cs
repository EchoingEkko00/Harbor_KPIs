using KpiService.Models;

namespace KpiService.KPIs;

public interface IKpiCalculator
{
    string KpiName { get; }
    Task<KpiResult> CalculateAsync();
}