﻿using MediatR;

using MiniERP.Application.Orders.Queries.GetById;

namespace MiniERP.RestApi.Endpoints.Orders;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new GetOrderByIdQuery(id));
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Orders);
    }
}
