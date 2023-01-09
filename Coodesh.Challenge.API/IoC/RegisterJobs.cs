using Coodesh.Challenge.API.Jobs;

namespace Coodesh.Challenge.API.IoC;

internal static class RegisterJobs
{
    public static void AddJobs(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<ImporterProductsJob>();
    }
}