using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Commands.Delete;

public sealed class DeleteProductCommandHandler(
    IRepository<Product> productRepository,
    IValidator<DeleteProductCommand> validator) : ICommandHandler<DeleteProductCommand>
{
    private readonly IRepository<Product> _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IValidator<DeleteProductCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var existingProductResult = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (existingProductResult.IsFailed && existingProductResult.HasErrorCode(ErrorCodes.Product.NotFound))
        {
            throw new ProductNotFoundException(command.ProductId);
        }

        return await _productRepository.DeleteAsync(command.ProductId, cancellationToken);
    }
}
