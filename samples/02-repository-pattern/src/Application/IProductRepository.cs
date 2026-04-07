using RepositoryPattern.Sample.Domain;

namespace RepositoryPattern.Sample.Application;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Product> AddAsync(Product product, CancellationToken ct);
}
