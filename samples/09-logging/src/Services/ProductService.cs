namespace Logging.Sample.Services;

public sealed class ProductService(ILogger<ProductService> logger)
{
    private static readonly Dictionary<int, (string Name, bool IsActive)> Products = new()
    {
        [1] = ("Laptop", true),
        [2] = ("Archived Monitor", false)
    };

    public object GetById(int id)
    {
        logger.LogDebug("Fetching product {ProductId}", id);

        try
        {
            if (!Products.TryGetValue(id, out var product))
            {
                throw new KeyNotFoundException($"Product {id} not found.");
            }

            logger.LogInformation("Product {ProductId} retrieved successfully", id);

            if (!product.IsActive)
            {
                logger.LogWarning("Product {ProductId} is inactive", id);
            }

            return new { id, product.Name, product.IsActive };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve product {ProductId}", id);
            throw;
        }
    }
}
