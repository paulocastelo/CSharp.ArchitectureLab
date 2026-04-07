using UnitTesting.Sample.Application;
using UnitTesting.Sample.Domain;

namespace UnitTesting.Sample.Infrastructure;

public sealed class InMemoryProductRepository : IProductRepository
{
    private static readonly Product Product = new()
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "Stock Product",
        CurrentBalance = 10
    };

    private static readonly List<StockMovement> Movements = [];

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(id == Product.Id ? Product : null);

    public Task SaveAsync(Product product, CancellationToken ct) => Task.CompletedTask;

    public Task AddMovementAsync(StockMovement movement, CancellationToken ct)
    {
        Movements.Add(movement);
        return Task.CompletedTask;
    }
}
