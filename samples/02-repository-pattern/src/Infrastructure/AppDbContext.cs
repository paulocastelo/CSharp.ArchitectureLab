using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Sample.Domain;

namespace RepositoryPattern.Sample.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
