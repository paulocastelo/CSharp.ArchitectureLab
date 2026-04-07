using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Sample.Application;

namespace OutboxPattern.Sample.Controllers;

[ApiController]
[Route("api")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpPost("products")]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var created = await productService.CreateAsync(request, ct);
        return Ok(created);
    }

    [HttpGet("outbox")]
    public async Task<IActionResult> GetOutbox(CancellationToken ct)
    {
        return Ok(await productService.GetOutboxAsync(ct));
    }
}
