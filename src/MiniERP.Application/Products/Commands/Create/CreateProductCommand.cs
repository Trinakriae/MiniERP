using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;

namespace MiniERP.Application.Products.Commands.Create
{
    public sealed record CreateProductCommand(
        ProductDto ProductDto
    ) : ICommand<int>;
}
