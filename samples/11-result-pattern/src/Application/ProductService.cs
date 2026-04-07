using ResultPattern.Sample.Common;
using ResultPattern.Sample.Domain;

namespace ResultPattern.Sample.Application;

public sealed class ProductService
{
    private readonly List<Product> _products =
    [
        new Product { Name = "Notebook", Sku = "RES-001", UnitPrice = 3200m }
    ];

    public Result<ProductResponse> Create(CreateProductRequest request)
    {
        if (_products.Any(product => product.Sku.Equals(request.Sku, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<ProductResponse>.Failure("SKU already exists.", "sku_conflict");
        }

        if (request.UnitPrice <= 0)
        {
            return Result<ProductResponse>.Failure("Unit price must be positive.", "invalid_price");
        }

        var product = new Product
        {
            Name = request.Name.Trim(),
            Sku = request.Sku.Trim(),
            UnitPrice = request.UnitPrice
        };

        _products.Add(product);
        return Result<ProductResponse>.Success(new ProductResponse(product.Id, product.Name, product.Sku, product.UnitPrice));
    }

    public IReadOnlyList<ProductResponse> GetAll() =>
        _products.Select(product => new ProductResponse(product.Id, product.Name, product.Sku, product.UnitPrice)).ToList();
}
