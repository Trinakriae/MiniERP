using MediatR;

using MiniERP.Application.Addresses.Queries.Get;

namespace MiniERP.RestApi.Endpoints.AddressBooks;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/addresses", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAddressesQuery());
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Addresses);
    }
}
