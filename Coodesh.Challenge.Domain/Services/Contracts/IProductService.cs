using Coodesh.Challenge.Domain.Entities;

namespace Coodesh.Challenge.Domain.Services.Contracts;

public interface IProductService
{
    public Task ScrapingAndUpdateAsync();

    public Task<Product> AddAsync(Product product);

    public Task SetDraftAsync(Guid productId);

    public Task<Product> GetProductAsync(long code);

    public Task<IEnumerable<Product>> GetProductsAsync(int take, int startPage);
}