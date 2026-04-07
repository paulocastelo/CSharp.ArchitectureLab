using UnitTesting.Sample.Domain;

namespace UnitTesting.Sample.Application;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveAsync(Product product, CancellationToken ct);
    Task AddMovementAsync(StockMovement movement, CancellationToken ct);
}
