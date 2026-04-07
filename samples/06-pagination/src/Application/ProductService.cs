using Microsoft.EntityFrameworkCore;
using Pagination.Sample.Common;
using Pagination.Sample.Domain;
using Pagination.Sample.Infrastructure;

namespace Pagination.Sample.Application;

public sealed class ProductService(AppDbContext dbContext)
{
    public async Task<PagedResult<Product>> GetPagedAsync(PaginationParams pagination, CancellationToken ct)
    {
        var query = dbContext.Products.AsNoTracking().OrderBy(p => p.Name);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Product>(items, pagination.Page, pagination.PageSize, total);
    }
}
