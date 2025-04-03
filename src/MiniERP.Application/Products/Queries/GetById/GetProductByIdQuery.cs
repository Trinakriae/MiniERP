using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;

namespace MiniERP.Application.Products.Queries.GetById;

public sealed record GetProductByIdQuery(
    int ProductId
) : IQuery<ProductDto>;
