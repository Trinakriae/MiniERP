using MiniERP.Application.Abstractions.Messaging;

namespace MiniERP.Application.Products.Commands.Delete;

public sealed record DeleteProductCommand(int ProductId) : ICommand;
