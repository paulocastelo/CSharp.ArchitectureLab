using Cqrs.Sample.Application.Contracts;
using MediatR;

namespace Cqrs.Sample.Application.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;
