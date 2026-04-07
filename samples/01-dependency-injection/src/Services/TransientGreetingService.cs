namespace DependencyInjection.Sample.Services;

public sealed class TransientGreetingService : ITransientGreetingService
{
    private readonly Guid _instanceId = Guid.NewGuid();

    public string Greet() => $"Transient: instance {_instanceId}";
}
