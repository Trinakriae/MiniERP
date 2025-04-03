using MediatR;

using MiniERP.Application.Orders.Commands.Update;
using MiniERP.RestApi.Abstractions.Orders;
using MiniERP.RestApi.Models.Orders;

namespace MiniERP.RestApi.Endpoints.Orders;

internal sealed class Update(IOrderRequestMapper<UpdateOrderRequest> orderRequestMapper) : IEndpoint
{
    private readonly IOrderRequestMapper<UpdateOrderRequest> _orderRequestMapper = orderRequestMapper
        ?? throw new ArgumentNullException(nameof(orderRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/orders/{id:int}", async (ISender sender, int id, UpdateOrderRequest request) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Order ID mismatch");
            }

            var orderDto = _orderRequestMapper.MapToOrderDto(request);
            var result = await sender.Send(new UpdateOrderCommand(orderDto));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .WithTags(Tags.Orders);
    }
}