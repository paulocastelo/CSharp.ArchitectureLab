using UnitTesting.Sample.Application.Contracts;
using UnitTesting.Sample.Domain;

namespace UnitTesting.Sample.Application;

public sealed class StockMovementService(IProductRepository productRepository)
{
    public async Task<int> CreateAsync(CreateMovementRequest request, CancellationToken ct = default)
    {
        if (request.Quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Quantity), "Quantity must be greater than zero.");
        }

        var product = await productRepository.GetByIdAsync(request.ProductId, ct);
        if (product is null)
        {
            throw new KeyNotFoundException($"Product with id '{request.ProductId}' was not found.");
        }

        if (request.Type == StockMovementType.Exit && product.CurrentBalance < request.Quantity)
        {
            throw new InvalidOperationException("Insufficient stock balance.");
        }

        product.CurrentBalance = request.Type == StockMovementType.Entry
            ? product.CurrentBalance + request.Quantity
            : product.CurrentBalance - request.Quantity;

        await productRepository.AddMovementAsync(new StockMovement
        {
            ProductId = product.Id,
            Quantity = request.Quantity,
            Type = request.Type
        }, ct);
        await productRepository.SaveAsync(product, ct);

        return product.CurrentBalance;
    }
}
