using Cqrs.Sample.Application.Contracts;
using Cqrs.Sample.Infrastructure;
using MediatR;

namespace Cqrs.Sample.Application.Queries;

public sealed class GetProductByIdQueryHandler(InMemoryProductRepository repository) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    public Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = repository.GetById(request.Id);
        return Task.FromResult(product is null ? null : new ProductResponse(product.Id, product.Name, product.Sku, product.UnitPrice));
    }
}
