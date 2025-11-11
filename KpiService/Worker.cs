namespace KpiService;

public class Worker : BackgroundService
{
    private readonly KpiRunner _runner;

    public Worker(KpiRunner runner)
    {
        _runner = runner;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _runner.RunAsync();
            await Task.Delay(TimeSpan.FromMinutes(1440), stoppingToken); // once a day for now
        }
    }
}
