using Microsoft.EntityFrameworkCore;
using OutboxPattern.Sample.Domain;

namespace OutboxPattern.Sample.Infrastructure;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
}
