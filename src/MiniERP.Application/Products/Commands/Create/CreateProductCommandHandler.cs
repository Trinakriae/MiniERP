using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Commands.Create
{
    public sealed class CreateProductCommandHandler(
        IRepository<Product> productRepository,
        IMapper<Product, ProductDto> productMapper,
        IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IRepository<Product> _productRepository = productRepository
                ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IMapper<Product, ProductDto> _productMapper = productMapper
                ?? throw new ArgumentNullException(nameof(productMapper));
        private readonly IValidator<CreateProductCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var product = _productMapper.Map(command.ProductDto);

            var addResult = await _productRepository.AddAsync(product, cancellationToken);
            if (addResult.IsFailed)
            {
                return Result.Fail(addResult.Errors);
            }

            return Result.Ok(addResult.Value);
        }
    }
}
