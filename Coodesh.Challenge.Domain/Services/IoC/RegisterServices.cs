using Coodesh.Challenge.Domain.Services.Contracts;
using Coodesh.Challenge.Domain.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Coodesh.Challenge.Domain.Services.IoC;

public static class RegisterDomainServices
{
    public static void AddDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IProductService, ProductService>();
    }
}