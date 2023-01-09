using NCrontab;

namespace Coodesh.Challenge.API.Jobs.Base;

internal abstract class BackgroundJob : BackgroundService
{
    private readonly string jobName;
    private readonly IServiceProvider serviceProvider;
    private readonly CrontabSchedule crontabSchedule;

    public BackgroundJob(string cronExpression, string jobName, IServiceProvider serviceProvider)
    {
        this.jobName = jobName;
        this.serviceProvider = serviceProvider;
        crontabSchedule = CrontabSchedule.Parse(cronExpression, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
    }

    protected abstract Task RunAsync(IServiceProvider serviceProvider);

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

            using var scope = serviceProvider.CreateScope();
            Console.WriteLine($"The job '{jobName}' is started.");

            try
            {
                await RunAsync(scope.ServiceProvider);
                Console.WriteLine($"The job '{jobName}' is finished with success.");
            }
            catch (Exception exc)
            {
                var error = exc.InnerException ?? exc;
                Console.WriteLine($"The job '{jobName}' is finished with error. '{error.Message}'.{Environment.NewLine}{error.StackTrace}");
            }
        }
    }
}