using KpiService.Models;

namespace KpiService.Repository;

public interface IKpiRepository
{
    Task UpsertKpiAsync(KpiResult result);
}