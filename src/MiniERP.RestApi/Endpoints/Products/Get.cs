using MediatR;

using MiniERP.Application.Products.Queries.Get;

namespace MiniERP.RestApi.Endpoints.Products;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Products);
    }
}
