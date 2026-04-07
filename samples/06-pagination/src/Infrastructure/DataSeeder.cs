using Pagination.Sample.Domain;

namespace Pagination.Sample.Infrastructure;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (dbContext.Products.Any())
        {
            return;
        }

        var products = Enumerable.Range(1, 100)
            .Select(index => new Product
            {
                Name = $"Product {index:000}",
                Sku = $"PAGE-{index:000}",
                UnitPrice = 10 + index
            })
            .ToList();

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }
}
