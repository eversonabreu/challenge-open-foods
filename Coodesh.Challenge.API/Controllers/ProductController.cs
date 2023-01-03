using Coodesh.Challenge.Domain.Entities;
using Coodesh.Challenge.Domain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Coodesh.Challenge.API.Controllers;

[Route("api/product")]
public sealed class ProductController : Controller
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public IActionResult GetDefaultMessage() => Ok("Fullstack Challenge 20201026");

    [HttpGet("{code:long}")]
    public async Task<IActionResult> GetProductAsync([FromRoute] long code)
    {
        var product = await productService.GetProductAsync(code);

        return Ok(product);
    }

    [HttpGet("{take:int}/{startPage:int}")]
    public async Task<IActionResult> GetProductsAsync([FromRoute] int take, [FromRoute] int startPage)
    {
        if (take < 1 || take > 100)
        {
            return BadRequest("Take results must between 1 and 100 value.");
        }

        if (startPage < 0)
        {
            return BadRequest("'StartPage' must positive value.");
        }

        var products = await productService.GetProductsAsync(take, startPage);

        return Ok(products);
    }

    [HttpPost("{code:long}")]
    public async Task<IActionResult> CreateAsync([FromRoute] long code)
    {
        var product = await productService.AddAsync(new Product { Code = code });

        if (product is null)
        {
            return BadRequest($"Product '{code}' has been added.");
        }

        return Ok(product);
    }

    [HttpPut("set-draft/{id:Guid}")]
    public async Task<IActionResult> SetDraftAsync([FromRoute] Guid id)
    {
        await productService.SetDraftAsync(id);

        return Ok();
    }
}