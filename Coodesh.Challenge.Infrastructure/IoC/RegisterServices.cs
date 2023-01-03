using Coodesh.Challenge.Domain.Repositories.Contracts;
using Coodesh.Challenge.Infrastructure.Database.Repositories.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Coodesh.Challenge.Infrastructure.IoC;

public static class RegisterDomainServices
{
    public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
    }
}