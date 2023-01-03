using Coodesh.Challenge.Domain.Entities;
using Coodesh.Challenge.Domain.Repositories.Contracts;
using Coodesh.Challenge.Infrastructure.Database.Repositories.Base;
using Microsoft.Extensions.Configuration;

namespace Coodesh.Challenge.Infrastructure.Database.Repositories.Implementations;

internal sealed class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IConfiguration configuration) : base(configuration) { }
}