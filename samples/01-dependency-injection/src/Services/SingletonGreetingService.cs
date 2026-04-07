namespace DependencyInjection.Sample.Services;

public sealed class SingletonGreetingService : ISingletonGreetingService
{
    private readonly Guid _instanceId = Guid.NewGuid();

    public string Greet() => $"Singleton: instance {_instanceId}";
}
