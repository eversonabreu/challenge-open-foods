using Coodesh.Challenge.Domain.Services.Contracts;
using NCrontab;

namespace Coodesh.Challenge.API.Jobs;

public sealed class ImporterProductsJob : BackgroundService
{
    private const string cron = "* 0 1 * * *"; //everyday 1 at morning
    private readonly CrontabSchedule crontabSchedule;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<ImporterProductsJob> logger;

    public ImporterProductsJob(IServiceProvider serviceProvider, 
        ILogger<ImporterProductsJob> logger)
    {
        this.serviceProvider = serviceProvider;
        crontabSchedule = CrontabSchedule.Parse(cron, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            var nextRun = crontabSchedule.GetNextOccurrence(DateTime.Now);
            var diffTime = nextRun.Subtract(DateTime.Now);
            await Task.Delay(diffTime, stoppingToken);

            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            await ScrapingAndUpdateAsync();
        }
    }

    private async Task ScrapingAndUpdateAsync()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
            await productService.ScrapingAndUpdateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed execution job importer products.");
        }
    }
}