using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Sample.Application;
using RepositoryPattern.Sample.Application.Contracts;

namespace RepositoryPattern.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await productService.GetAllAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await productService.GetByIdAsync(id, ct);
        return product is null ? NotFound(new { detail = $"Product with id '{id}' was not found." }) : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var product = await productService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}
