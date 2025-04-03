using MediatR;

using MiniERP.Application.Addresses.Queries.GetById;

namespace MiniERP.RestApi.Endpoints.AddressBooks;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/addresses/{id:int}", async (ISender sender, int id) =>
        {
            var result = await sender.Send(new GetAddressByIdQuery(id));
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound(result.Errors.Select(e => e.Message));
        })
        .WithTags(Tags.Addresses);
    }
}
