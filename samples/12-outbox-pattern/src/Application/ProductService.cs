using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OutboxPattern.Sample.Domain;
using OutboxPattern.Sample.Infrastructure;

namespace OutboxPattern.Sample.Application;

public sealed class ProductService(AppDbContext dbContext)
{
    public async Task<object> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        var product = new Product
        {
            Name = request.Name.Trim(),
            Sku = request.Sku.Trim(),
            UnitPrice = request.UnitPrice
        };

        dbContext.Products.Add(product);
        dbContext.OutboxMessages.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = "ProductCreated",
            Payload = JsonSerializer.Serialize(new { product.Id, product.Name }),
            CreatedAtUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        return new { product.Id, product.Name, product.Sku, product.UnitPrice };
    }

    public async Task<IReadOnlyList<object>> GetOutboxAsync(CancellationToken ct)
    {
        return await dbContext.OutboxMessages
            .AsNoTracking()
            .OrderByDescending(message => message.CreatedAtUtc)
            .Select(message => new
            {
                message.Id,
                message.EventType,
                message.Payload,
                message.CreatedAtUtc,
                message.ProcessedAtUtc,
                status = message.ProcessedAtUtc == null ? "pending" : "processed"
            })
            .ToListAsync<object>(ct);
    }
}
