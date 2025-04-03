using MediatR;

using MiniERP.Application.Users.Commands.Update;
using MiniERP.RestApi.Abstractions.Users;
using MiniERP.RestApi.Models.Users;

namespace MiniERP.RestApi.Endpoints.Users;

internal sealed class Update(IUserRequestMapper<UpdateUserRequest> userRequestMapper) : IEndpoint
{
    private readonly IUserRequestMapper<UpdateUserRequest> _userRequestMapper = userRequestMapper
        ?? throw new ArgumentNullException(nameof(userRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{id:int}", async (ISender sender, int id, UpdateUserRequest request) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("User ID mismatch");
            }

            var userDto = _userRequestMapper.MapToUserDto(request);
            var result = await sender.Send(new UpdateUserCommand(userDto));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .WithTags(Tags.Users);
    }
}
