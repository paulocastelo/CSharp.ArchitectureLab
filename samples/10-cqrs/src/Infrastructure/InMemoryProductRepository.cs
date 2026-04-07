using Cqrs.Sample.Domain;

namespace Cqrs.Sample.Infrastructure;

public sealed class InMemoryProductRepository
{
    private readonly List<Product> _products =
    [
        new Product { Name = "Desk", Sku = "CQRS-001", UnitPrice = 900m },
        new Product { Name = "Chair", Sku = "CQRS-002", UnitPrice = 450m }
    ];

    public IReadOnlyList<Product> GetAll() => _products.OrderBy(p => p.Name).ToList();

    public Product? GetById(Guid id) => _products.FirstOrDefault(p => p.Id == id);

    public Product Add(Product product)
    {
        _products.Add(product);
        return product;
    }
}
