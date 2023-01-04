using Coodesh.Challenge.Domain.Entities;
using Coodesh.Challenge.Domain.Repositories.Contracts;
using Coodesh.Challenge.Domain.Services.Implementations;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Coodesh.Challenge.UnitTests.Services;

public sealed class ProductServiceTests
{
    private readonly IProductRepository productRepositorySubstitute;
    private readonly IConfiguration configurationSubstitute;
    private readonly ProductService productServiceSubstitute;

    public ProductServiceTests()
    {
        productRepositorySubstitute = Substitute.For<IProductRepository>();
        configurationSubstitute = Substitute.For<IConfiguration>();
        productServiceSubstitute = Substitute.For<ProductService>(productRepositorySubstitute, configurationSubstitute);
    }

    [Fact]
    public async Task ScrapingAndUpdateAsyncCorrect_Execution()
    {
        //arrange

        productRepositorySubstitute.GetByFilterAsync(x => x.Status == Domain.Enums.ProductStatus.Draft)
            .ReturnsForAnyArgs(
                new List<Product>
                {
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Code = 7622210449283,
                        Status = Domain.Enums.ProductStatus.Draft
                    },
                    new Product
                    {
                        Id = Guid.NewGuid(),
                        Code = 5449000000996,
                        Status = Domain.Enums.ProductStatus.Draft
                    }
                });

        configurationSubstitute.GetSection("UrlSite").Value.Returns("https://world.openfoodfacts.org/product/{0}");
        
        // actual

        await productServiceSubstitute.ScrapingAndUpdateAsync();

        // assert

        //test for two calls (return two products)
        productRepositorySubstitute.Received(2).Update(Arg.Any<Product>());

        //test one call 'CommitAsync'
        await productRepositorySubstitute.Received(1).CommitAsync();
    }

    [Fact]
    public async Task ScrapingAndUpdateAsync_Not_Received_CommitAsync_When_Not_Found_Products_In_Draft()
    {
        //arrange

        productRepositorySubstitute.GetByFilterAsync(x => x.Status == Domain.Enums.ProductStatus.Draft)
            .ReturnsForAnyArgs(Enumerable.Empty<Product>().ToList());

        // actual

        await productServiceSubstitute.ScrapingAndUpdateAsync();

        // assert

        //test not the call 'CommitAsync'
        await productRepositorySubstitute.DidNotReceive().CommitAsync();
    }
}