using AutoMapper;
using DtoMapping.Sample.Application.Contracts;
using DtoMapping.Sample.Application.Extensions;
using DtoMapping.Sample.Domain;

namespace DtoMapping.Sample.Application;

public sealed class ProductService(IMapper mapper)
{
    private static readonly List<Product> Products =
    [
        new Product { Name = "Monitor", Sku = "DTO-001", InternalCost = 780m, InternalNotes = "Supplier margin 18%" },
        new Product { Name = "Keyboard", Sku = "DTO-002", InternalCost = 150m, InternalNotes = "Keep internal notes private" }
    ];

    public IReadOnlyList<ProductResponse> GetManual() =>
        Products.Select(p => p.ToResponse()).ToList();

    public IReadOnlyList<ProductResponse> GetAutoMapped() =>
        mapper.Map<IReadOnlyList<ProductResponse>>(Products);

    public ProductResponse Create(CreateProductRequest request)
    {
        var product = mapper.Map<Product>(request);
        product.Id = Guid.NewGuid();
        Products.Add(product);
        return product.ToResponse();
    }
}
