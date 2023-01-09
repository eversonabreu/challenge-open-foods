using Coodesh.Challenge.API.Jobs.Base;
using Coodesh.Challenge.Domain.Services.Contracts;

namespace Coodesh.Challenge.API.Jobs;

internal sealed class ImporterProductsJob : BackgroundJob
{
    //everyday 1 at morning
    private const string cronExpression = "* 0 1 * * *";

    public ImporterProductsJob(IServiceProvider serviceProvider)
        : base(cronExpression, nameof(ImporterProductsJob), serviceProvider) { }

    protected override async Task RunAsync(IServiceProvider serviceProvider)
    {
        var productService = serviceProvider.GetRequiredService<IProductService>();
        await productService.ScrapingAndUpdateAsync();
    }
}