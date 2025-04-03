using MediatR;

using MiniERP.Application.Users.Queries.Get;

namespace MiniERP.RestApi.Endpoints.Users;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery());
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Users);
    }
}
