using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Sample.Application;
using RepositoryPattern.Sample.Domain;

namespace RepositoryPattern.Sample.Infrastructure;

public sealed class EfCoreProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct) =>
        await dbContext.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct)!;

    public async Task<Product> AddAsync(Product product, CancellationToken ct)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(ct);
        return product;
    }
}
