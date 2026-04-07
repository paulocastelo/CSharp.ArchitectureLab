using DependencyInjection.Sample.Services;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LifetimeController : ControllerBase
{
    private readonly ITransientGreetingService _transient1;
    private readonly ITransientGreetingService _transient2;
    private readonly IScopedGreetingService _scoped1;
    private readonly IScopedGreetingService _scoped2;
    private readonly ISingletonGreetingService _singleton;

    public LifetimeController(
        ITransientGreetingService transient1,
        ITransientGreetingService transient2,
        IScopedGreetingService scoped1,
        IScopedGreetingService scoped2,
        ISingletonGreetingService singleton)
    {
        _transient1 = transient1;
        _transient2 = transient2;
        _scoped1 = scoped1;
        _scoped2 = scoped2;
        _singleton = singleton;
    }

    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        transient1 = _transient1.Greet(),
        transient2 = _transient2.Greet(),
        scoped1 = _scoped1.Greet(),
        scoped2 = _scoped2.Greet(),
        singleton = _singleton.Greet()
    });
}
