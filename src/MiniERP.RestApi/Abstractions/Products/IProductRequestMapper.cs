using MiniERP.Application.Products.Dtos;

namespace MiniERP.RestApi.Abstractions.Products;

public interface IProductRequestMapper<TRequest>
{
    ProductDto MapToProductDto(TRequest request);
}

