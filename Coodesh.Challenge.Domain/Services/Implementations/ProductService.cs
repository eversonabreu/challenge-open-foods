using Coodesh.Challenge.Domain.Entities;
using Coodesh.Challenge.Domain.Repositories.Contracts;
using Coodesh.Challenge.Domain.Services.Contracts;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace Coodesh.Challenge.Domain.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository productRepository;
    private readonly IConfiguration configuration;

    public ProductService(IProductRepository productRepository,
        IConfiguration configuration)
    {
        this.productRepository = productRepository;
        this.configuration = configuration;
    }

    public async Task ScrapingAndUpdateAsync()
    {
        var products = await productRepository.GetByFilterAsync(x => x.Status == Enums.ProductStatus.Draft);

        if (products.Count == 0)
        {
            return;
        }

        foreach (var product in products)
        {
            var urlSite = string.Format(configuration.GetSection("UrlSite").Value, product.Code);
            var htmlDocument = GetHtmlDocument(urlSite);

            if (htmlDocument is null)
            {
                continue;
            }

            product.URL = urlSite;
            product.Status = Enums.ProductStatus.Imported;
            product.ImportedDate = DateTime.Now;
            product.Name = GetNameOrDefault(product.Name, htmlDocument);
            product.BarCode = GetElementByIdOrDefault("barcode_paragraph", htmlDocument, product.BarCode)?.Replace("Barcode:", string.Empty).Trim();
            product.Packaging = GetElementByIdOrDefault("field_packaging_value", htmlDocument);
            product.Brands = GetElementByIdOrDefault("field_brands_value", htmlDocument);
            product.Quantity = GetElementByIdOrDefault("field_quantity_value", htmlDocument);
            product.Categories = GetElementByIdOrDefault("field_categories_value", htmlDocument);
            product.ImageURL = GetImageOrDefault(htmlDocument);

            productRepository.Update(product);

        }

        await productRepository.CommitAsync();
    }

    private static HtmlDocument GetHtmlDocument(string urlSite)
    {
        try
        {
            var htmlWeb = new HtmlWeb();
            var htmlDocument = htmlWeb.Load(urlSite);
            return htmlDocument;
        }
        catch
        {
            return null;
        }
    }

    private static string ClearValue(string value)
    {
        return value
            .Trim()
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\t", string.Empty);
    }

    private static string GetNameOrDefault(string defaultName, HtmlDocument htmlDocument)
    {
        try
        {
            var result = ClearValue(htmlDocument.DocumentNode.SelectSingleNode("//h2[@class='title-1']").InnerText.Split("\n").First());
            return result;
        }
        catch
        {
            return defaultName;
        }
    }

    private static string GetElementByIdOrDefault(string elementId, HtmlDocument htmlDocument, string defaultValue = null)
    {
        try
        {
            var result = ClearValue(htmlDocument.GetElementbyId(elementId).InnerText);
            return result;
        }
        catch
        {
            return defaultValue;
        }
    }

    private static string GetImageOrDefault(HtmlDocument htmlDocument)
    {
        try
        {
            var result = htmlDocument.GetElementbyId("og_image").Attributes["src"].Value.Trim();
            return result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<Product> AddAsync(Product product)
    {
        var exists = await productRepository.SingleOrDefaultAsync(x => x.Code == product.Code);

        if (exists != null)
        {
            return null;
        }

        product.Status = Enums.ProductStatus.Draft;
        var result = await productRepository.AddAsync(product);
        await productRepository.CommitAsync();
        return result;
    }

    public async Task SetDraftAsync(Guid productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        product.Status = Enums.ProductStatus.Draft;
        productRepository.Update(product);
        await productRepository.CommitAsync();
    }

    public async Task<Product> GetProductAsync(long code) => await productRepository.SingleOrDefaultAsync(x => x.Code == code);

    public async Task<IEnumerable<Product>> GetProductsAsync(int take, int startPage)
    {
        var products = await productRepository.GetByFilterAsync(x => true, take, startPage);
        products = products.OrderBy(x => x.Name).ToList();
        return products;
    }
}