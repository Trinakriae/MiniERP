using MediatR;

using MiniERP.Application.Users.Commands.Create;
using MiniERP.RestApi.Abstractions.Users;
using MiniERP.RestApi.Models.Users;

namespace MiniERP.RestApi.Endpoints.Users;

internal sealed class Create(IUserRequestMapper<CreateUserRequest> userRequestMapper) : IEndpoint
{
    private readonly IUserRequestMapper<CreateUserRequest> _userRequestMapper = userRequestMapper
        ?? throw new ArgumentNullException(nameof(userRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (ISender sender, CreateUserRequest request) =>
        {
            var userDto = _userRequestMapper.MapToUserDto(request);
            var result = await sender.Send(new CreateUserCommand(userDto));

            return result.IsSuccess
                ? Results.Created($"/users/{result.Value}", result.Value)
                : Results.BadRequest(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Users);
    }
}
