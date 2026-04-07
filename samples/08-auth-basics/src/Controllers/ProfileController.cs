using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthBasics.Sample.Controllers;

[ApiController]
[Authorize]
[Route("api/profile")]
public sealed class ProfileController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProfile()
    {
        return Ok(new
        {
            id = User.FindFirst("userId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            email = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("sub")?.Value,
            fullName = User.FindFirst("fullName")?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value
        });
    }
}
