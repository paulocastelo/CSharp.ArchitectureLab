using Cqrs.Sample.Application.Contracts;
using Cqrs.Sample.Infrastructure;
using MediatR;

namespace Cqrs.Sample.Application.Queries;

public sealed class GetProductsQueryHandler(InMemoryProductRepository repository) : IRequestHandler<GetProductsQuery, PagedResult<ProductResponse>>
{
    public Task<PagedResult<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var all = repository.GetAll()
            .Select(p => new ProductResponse(p.Id, p.Name, p.Sku, p.UnitPrice))
            .ToList();

        var items = all.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
        return Task.FromResult(new PagedResult<ProductResponse>(items, request.Page, request.PageSize, all.Count));
    }
}
