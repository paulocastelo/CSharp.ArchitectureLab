using ErrorHandling.Sample.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ErrorHandling.Sample.Controllers;

[ApiController]
[Route("api/demo")]
public sealed class DemoController : ControllerBase
{
    [HttpGet("not-found")]
    public IActionResult ThrowNotFound() => throw new NotFoundException("Product with id 'abc-123' was not found.");

    [HttpGet("conflict")]
    public IActionResult ThrowConflict() => throw new ConflictException("A product with the same SKU already exists.");

    [HttpGet("business-rule")]
    public IActionResult ThrowBusinessRule() => throw new BusinessRuleException("Stock balance cannot become negative.");

    [HttpGet("unhandled")]
    public IActionResult ThrowUnhandled() => throw new InvalidOperationException("This simulates an unexpected infrastructure failure.");
}
