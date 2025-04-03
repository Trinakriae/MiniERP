using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;

namespace MiniERP.Application.Products.Queries.Get;

public sealed record GetProductsQuery() : IQuery<IEnumerable<ProductDto>>;


