using DtoMapping.Sample.Application;
using DtoMapping.Sample.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DtoMapping.Sample.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductService productService) : ControllerBase
{
    [HttpGet("manual")]
    public IActionResult GetManual() => Ok(productService.GetManual());

    [HttpGet("automapper")]
    public IActionResult GetAutoMapper() => Ok(productService.GetAutoMapped());

    [HttpPost]
    public IActionResult Create([FromBody] CreateProductRequest request)
    {
        var created = productService.Create(request);
        return Created($"/api/products/manual/{created.Id}", created);
    }
}
