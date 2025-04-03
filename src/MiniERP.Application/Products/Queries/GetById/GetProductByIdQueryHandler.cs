using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Queries.GetById;

public sealed class GetProductByIdQueryHandler(
    IRepository<Product> productRepository,
    IRepository<Category> categoryRepository,
    IMapper<Product, ProductDto> productMapper,
    IMapper<Category, CategoryDto> categoryMapper) : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IRepository<Product> _productRepository = productRepository
            ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IRepository<Category> _categoryRepository = categoryRepository
            ?? throw new ArgumentNullException(nameof(categoryRepository));
    private readonly IMapper<Product, ProductDto> _productMapper = productMapper
            ?? throw new ArgumentNullException(nameof(productMapper));
    private readonly IMapper<Category, CategoryDto> _categoryMapper = categoryMapper
            ?? throw new ArgumentNullException(nameof(categoryMapper));

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var productResult = await _productRepository.GetByIdAsync(query.ProductId, cancellationToken);
        if (productResult.IsFailed)
        {
            return Result.Fail(productResult.Errors);
        }

        var product = productResult.Value;
        var categoryResult = await _categoryRepository.GetByIdAsync(product.CategoryId, cancellationToken);
        if (categoryResult.IsFailed)
        {
            return Result.Fail(categoryResult.Errors);
        }

        var productDto = _productMapper.Map(product);
        productDto.Category = _categoryMapper.Map(categoryResult.Value);

        return Result.Ok(productDto);
    }
}


