using MediatR;

using MiniERP.Application.Addresses.Commands.Update;
using MiniERP.RestApi.Abstractions.AddressBooks;
using MiniERP.RestApi.Models.AddressBooks;

namespace MiniERP.RestApi.Endpoints.AddressBooks;

internal sealed class Update(IAddressRequestMapper<UpdateAddressRequest> addressRequestMapper) : IEndpoint
{
    private readonly IAddressRequestMapper<UpdateAddressRequest> _addressRequestMapper = addressRequestMapper
        ?? throw new ArgumentNullException(nameof(addressRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/addresses/{id:int}", async (ISender sender, int id, UpdateAddressRequest request) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Address ID mismatch");
            }

            var addressDto = _addressRequestMapper.MapToAddressDto(request);
            var result = await sender.Send(new UpdateAddressCommand(addressDto));
            return result.IsSuccess
                ? Results.NoContent()
                : Results.BadRequest(result.Errors);
        })
        .WithTags(Tags.Addresses);
    }
}
