using Microsoft.AspNetCore.Mvc;
using ResultPattern.Sample.Application;

namespace ResultPattern.Sample.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok(productService.GetAll());

    [HttpPost]
    public IActionResult Create([FromBody] CreateProductRequest request)
    {
        var result = productService.CreateAsync(request);
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "sku_conflict" => Conflict(new { detail = result.Error }),
                "not_found" => NotFound(new { detail = result.Error }),
                _ => BadRequest(new { detail = result.Error })
            };
        }

        return Ok(result.Value);
    }
}
