﻿using MediatR;

using MiniERP.Application.Users.Commands.Delete;

namespace MiniERP.RestApi.Endpoints.Users;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/users/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new DeleteUserCommand(id));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Users);
    }
}
