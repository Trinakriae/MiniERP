using MiniERP.Application.Products.Dtos;
using MiniERP.RestApi.Abstractions.Products;
using MiniERP.RestApi.Models.Products;

namespace MiniERP.RestApi.Mappers.Products;

public class CreateProductRequestMapper : IProductRequestMapper<CreateProductRequest>
{
    public ProductDto MapToProductDto(CreateProductRequest request) => new()
    {
        Name = request.Name,
        Description = request.Description,
        UnitPrice = request.UnitPrice,
        Category = new CategoryDto { Id = request.CategoryId }
    };
}

