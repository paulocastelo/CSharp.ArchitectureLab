using RepositoryPattern.Sample.Application.Contracts;
using RepositoryPattern.Sample.Domain;

namespace RepositoryPattern.Sample.Application;

public sealed class ProductService(IProductRepository repository)
{
    public async Task<IReadOnlyList<ProductResponse>> GetAllAsync(CancellationToken ct)
    {
        var products = await repository.GetAllAsync(ct);
        return products.Select(Map).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var product = await repository.GetByIdAsync(id, ct);
        return product is null ? null : Map(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        var product = new Product
        {
            Name = request.Name.Trim(),
            Sku = request.Sku.Trim(),
            UnitPrice = request.UnitPrice
        };

        var saved = await repository.AddAsync(product, ct);
        return Map(saved);
    }

    private static ProductResponse Map(Product product) =>
        new(product.Id, product.Name, product.Sku, product.UnitPrice);
}
