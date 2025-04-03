using MediatR;

using MiniERP.Application.Orders.Queries.Get;

namespace MiniERP.RestApi.Endpoints.Orders;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders", async (ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersQuery());
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Orders);
    }
}
