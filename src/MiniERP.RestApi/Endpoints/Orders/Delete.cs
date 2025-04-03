using MediatR;

using MiniERP.Application.Orders.Commands.Delete;

namespace MiniERP.RestApi.Endpoints.Orders;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/orders/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(id));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Orders);
    }
}
