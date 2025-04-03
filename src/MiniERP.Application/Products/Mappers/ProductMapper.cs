using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Mappers;

public class ProductMapper : IMapper<Product, ProductDto>
{
    public Product Map(ProductDto productDto)
    {
        _ = productDto ?? throw new ArgumentException("ProductDto cannot be null");

        return new Product
        {
            Id = productDto.Id ?? default,
            Name = productDto.Name,
            Description = productDto.Description,
            UnitPrice = productDto.UnitPrice,
            CategoryId = productDto.Category.Id,
        };
    }

    public ProductDto Map(Product product)
    {
        _ = product ?? throw new ArgumentException("Product cannot be null");

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            Category = new CategoryDto { Id = product.CategoryId }
        };
    }
}


