using AuthBasics.Sample.Application;
using AuthBasics.Sample.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthBasics.Sample.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await authService.RegisterAsync(request);
            return StatusCode(StatusCodes.Status201Created, user);
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { detail = exception.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            return Ok(await authService.LoginAsync(request));
        }
        catch (UnauthorizedAccessException exception)
        {
            return Unauthorized(new { detail = exception.Message });
        }
    }
}
