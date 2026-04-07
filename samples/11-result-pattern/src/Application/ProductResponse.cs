namespace ResultPattern.Sample.Application;

public sealed record ProductResponse(Guid Id, string Name, string Sku, decimal UnitPrice);
