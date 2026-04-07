using Microsoft.AspNetCore.Mvc;
using UnitTesting.Sample.Application;
using UnitTesting.Sample.Application.Contracts;

namespace UnitTesting.Sample.Controllers;

[ApiController]
[Route("api/movements")]
public sealed class MovementsController(StockMovementService stockMovementService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovementRequest request, CancellationToken ct)
    {
        var balance = await stockMovementService.CreateAsync(request, ct);
        return Ok(new { currentBalance = balance });
    }
}
