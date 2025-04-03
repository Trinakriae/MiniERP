using MediatR;

using MiniERP.Application.Products.Queries.GetById;

namespace MiniERP.RestApi.Endpoints.Products;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Products);
    }
}
