using Cqrs.Sample.Application.Contracts;
using MediatR;

namespace Cqrs.Sample.Application.Queries;

public sealed record GetProductsQuery(int Page, int PageSize) : IRequest<PagedResult<ProductResponse>>;
