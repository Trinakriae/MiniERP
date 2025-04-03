using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Queries.Get
{
    public sealed class GetProductsQueryHandler(
        IRepository<Product> productRepository,
        IRepository<Category> categoryRepository,
        IMapper<Product, ProductDto> productMapper,
        IMapper<Category, CategoryDto> categoryMapper) : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IRepository<Product> _productRepository = productRepository
                ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IRepository<Category> _categoryRepository = categoryRepository
                ?? throw new ArgumentNullException(nameof(categoryRepository));
        private readonly IMapper<Product, ProductDto> _productMapper = productMapper
                ?? throw new ArgumentNullException(nameof(productMapper));
        private readonly IMapper<Category, CategoryDto> _categoryMapper = categoryMapper
                ?? throw new ArgumentNullException(nameof(categoryMapper));

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(query);

            var productsResult = await _productRepository.GetAsync(cancellationToken);
            if (productsResult.IsFailed)
            {
                return Result.Fail(productsResult.Errors);
            }

            var productDtos = new List<ProductDto>();

            foreach (var product in productsResult.Value)
            {
                var categoryResult = await _categoryRepository.GetByIdAsync(product.CategoryId, cancellationToken);
                if (categoryResult.IsFailed)
                {
                    return Result.Fail(categoryResult.Errors);
                }

                var productDto = _productMapper.Map(product);
                productDto.Category = _categoryMapper.Map(categoryResult.Value);
                productDtos.Add(productDto);
            }

            return Result.Ok(productDtos.AsEnumerable());
        }
    }
}


