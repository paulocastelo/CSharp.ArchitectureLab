namespace RepositoryPattern.Sample.Application.Contracts;

public sealed record ProductResponse(Guid Id, string Name, string Sku, decimal UnitPrice);
