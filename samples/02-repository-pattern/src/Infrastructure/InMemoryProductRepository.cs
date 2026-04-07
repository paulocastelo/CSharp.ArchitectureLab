using RepositoryPattern.Sample.Application;
using RepositoryPattern.Sample.Domain;

namespace RepositoryPattern.Sample.Infrastructure;

public sealed class InMemoryProductRepository : IProductRepository
{
    private static readonly List<Product> Products =
    [
        new Product { Name = "Laptop", Sku = "REP-001", UnitPrice = 4200m },
        new Product { Name = "Mouse", Sku = "REP-002", UnitPrice = 120m }
    ];

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<Product>>(Products.ToList());

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(Products.FirstOrDefault(p => p.Id == id));

    public Task<Product> AddAsync(Product product, CancellationToken ct)
    {
        Products.Add(product);
        return Task.FromResult(product);
    }
}
