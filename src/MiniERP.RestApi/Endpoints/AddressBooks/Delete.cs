using MediatR;

using MiniERP.Application.Addresses.Commands.Delete;

namespace MiniERP.RestApi.Endpoints.AddressBooks;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/addresses/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new DeleteAddressCommand(id));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Addresses);
    }
}
