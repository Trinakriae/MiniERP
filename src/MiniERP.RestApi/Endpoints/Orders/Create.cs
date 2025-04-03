using MediatR;

using MiniERP.Application.Orders.Commands.Create;
using MiniERP.RestApi.Abstractions.Orders;
using MiniERP.RestApi.Models.Orders;

namespace MiniERP.RestApi.Endpoints.Orders;

internal sealed class Create(IOrderRequestMapper<CreateOrderRequest> orderRequestMapper) : IEndpoint
{
    private readonly IOrderRequestMapper<CreateOrderRequest> _orderRequestMapper = orderRequestMapper
        ?? throw new ArgumentNullException(nameof(orderRequestMapper));


    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders", async (ISender sender, CreateOrderRequest request) =>
        {
            var orderDto = _orderRequestMapper.MapToOrderDto(request);
            var result = await sender.Send(new CreateOrderCommand(orderDto));

            return result.IsSuccess
                ? Results.Created($"/orders/{result.Value}", result.Value)
                : Results.BadRequest(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Orders);
    }
}
