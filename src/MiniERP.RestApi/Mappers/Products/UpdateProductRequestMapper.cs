using MiniERP.Application.Products.Dtos;
using MiniERP.RestApi.Abstractions.Products;
using MiniERP.RestApi.Models.Products;

namespace MiniERP.RestApi.Mappers.Products;

public class UpdateProductRequestMapper : IProductRequestMapper<UpdateProductRequest>
{
    public ProductDto MapToProductDto(UpdateProductRequest request) => new()
    {
        Id = request.Id,
        Name = request.Name,
        Description = request.Description,
        UnitPrice = request.UnitPrice,
        Category = new CategoryDto { Id = request.CategoryId }
    };
}

