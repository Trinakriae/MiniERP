using MediatR;

using MiniERP.Application.Products.Commands.Delete;

namespace MiniERP.RestApi.Endpoints.Products;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/products/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Products);
    }
}
