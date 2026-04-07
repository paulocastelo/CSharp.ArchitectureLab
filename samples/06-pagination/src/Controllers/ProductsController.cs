using Microsoft.AspNetCore.Mvc;
using Pagination.Sample.Application;
using Pagination.Sample.Common;

namespace Pagination.Sample.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationParams pagination, CancellationToken ct)
    {
        var result = await productService.GetPagedAsync(pagination, ct);
        return Ok(result);
    }
}
