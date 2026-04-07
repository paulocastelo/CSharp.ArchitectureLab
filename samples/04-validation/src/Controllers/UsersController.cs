using Microsoft.AspNetCore.Mvc;
using Validation.Sample.Contracts;

namespace Validation.Sample.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        return Ok(new
        {
            message = "User registration request accepted.",
            user = new
            {
                request.FullName,
                request.Email
            }
        });
    }
}
