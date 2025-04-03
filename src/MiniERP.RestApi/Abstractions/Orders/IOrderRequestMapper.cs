using MiniERP.Application.Orders.Dtos;

namespace MiniERP.RestApi.Abstractions.Orders;

public interface IOrderRequestMapper<TRequest>
{
    OrderDto MapToOrderDto(TRequest request);
}