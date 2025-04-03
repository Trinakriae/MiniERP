using MediatR;

using MiniERP.Application.Users.Queries.GetById;

namespace MiniERP.RestApi.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new GetUserByIdQuery(id));
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Users);
    }
}
