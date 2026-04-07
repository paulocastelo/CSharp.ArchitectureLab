using Cqrs.Sample.Application.Contracts;
using Cqrs.Sample.Domain;
using Cqrs.Sample.Infrastructure;
using MediatR;

namespace Cqrs.Sample.Application.Commands;

public sealed class CreateProductCommandHandler(InMemoryProductRepository repository) : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = repository.Add(new Product
        {
            Name = request.Name.Trim(),
            Sku = request.Sku.Trim(),
            UnitPrice = request.UnitPrice
        });

        return Task.FromResult(new ProductResponse(product.Id, product.Name, product.Sku, product.UnitPrice));
    }
}
