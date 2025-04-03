using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;

namespace MiniERP.Application.Products.Commands.Update
{
    public sealed record UpdateProductCommand(
        ProductDto ProductDto
    ) : ICommand;
}
