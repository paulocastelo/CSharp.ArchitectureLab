using Cqrs.Sample.Application.Contracts;
using MediatR;

namespace Cqrs.Sample.Application.Commands;

public sealed record CreateProductCommand(string Name, string Sku, decimal UnitPrice) : IRequest<ProductResponse>;
