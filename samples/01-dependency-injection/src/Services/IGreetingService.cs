namespace DependencyInjection.Sample.Services;

public interface IGreetingService
{
    string Greet();
}

public interface ITransientGreetingService : IGreetingService;

public interface IScopedGreetingService : IGreetingService;

public interface ISingletonGreetingService : IGreetingService;
