using MediatR;

using MiniERP.Application.Products.Commands.Create;
using MiniERP.RestApi.Abstractions.Products;
using MiniERP.RestApi.Models.Products;

namespace MiniERP.RestApi.Endpoints.Products;

internal sealed class Create(IProductRequestMapper<CreateProductRequest> productRequestMapper) : IEndpoint
{
    private readonly IProductRequestMapper<CreateProductRequest> _productRequestMapper = productRequestMapper
        ?? throw new ArgumentNullException(nameof(productRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", async (ISender sender, CreateProductRequest request) =>
        {
            var productDto = _productRequestMapper.MapToProductDto(request);
            var result = await sender.Send(new CreateProductCommand(productDto));

            return result.IsSuccess
                ? Results.Created($"/products/{result.Value}", result.Value)
                : Results.BadRequest(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Products);
    }
}
