using MediatR;

using MiniERP.Application.Products.Commands.Update;
using MiniERP.RestApi.Abstractions.Products;
using MiniERP.RestApi.Models.Products;

namespace MiniERP.RestApi.Endpoints.Products;

internal sealed class Update(IProductRequestMapper<UpdateProductRequest> productRequestMapper) : IEndpoint
{
    private readonly IProductRequestMapper<UpdateProductRequest> _productRequestMapper = productRequestMapper
        ?? throw new ArgumentNullException(nameof(productRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/products/{id:int}", async (ISender sender, int id, UpdateProductRequest request) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Product ID mismatch");
            }

            var productDto = _productRequestMapper.MapToProductDto(request);
            var result = await sender.Send(new UpdateProductCommand(productDto));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .WithTags(Tags.Products);
    }
}
