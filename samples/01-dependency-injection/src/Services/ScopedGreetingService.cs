namespace DependencyInjection.Sample.Services;

public sealed class ScopedGreetingService : IScopedGreetingService
{
    private readonly Guid _instanceId = Guid.NewGuid();

    public string Greet() => $"Scoped: instance {_instanceId}";
}
