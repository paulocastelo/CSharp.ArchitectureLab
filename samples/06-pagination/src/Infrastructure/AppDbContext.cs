using Microsoft.EntityFrameworkCore;
using Pagination.Sample.Domain;

namespace Pagination.Sample.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
