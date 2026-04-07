using Cqrs.Sample.Application.Commands;
using Cqrs.Sample.Application.Contracts;
using Cqrs.Sample.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cqrs.Sample.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Sku, request.UnitPrice);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id));
        return result is null ? NotFound(new { detail = $"Product with id '{id}' was not found." }) : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetProductsQuery(page, pageSize));
        return Ok(result);
    }
}
