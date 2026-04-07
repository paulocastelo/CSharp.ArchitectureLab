using Microsoft.AspNetCore.Mvc;
using Validation.Sample.Contracts;

namespace Validation.Sample.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateProductRequest request)
    {
        return Ok(new
        {
            message = "Product request accepted.",
            product = request
        });
    }
}
