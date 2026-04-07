using DtoMapping.Sample.Application.Contracts;
using DtoMapping.Sample.Domain;

namespace DtoMapping.Sample.Application.Extensions;

public static class ProductMappingExtensions
{
    public static ProductResponse ToResponse(this Product product) =>
        new(product.Id, product.Name, product.Sku);
}
