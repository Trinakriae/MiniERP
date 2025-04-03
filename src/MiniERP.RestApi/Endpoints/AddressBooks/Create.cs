using MediatR;

using MiniERP.Application.Addresses.Commands.Create;
using MiniERP.RestApi.Abstractions.AddressBooks;
using MiniERP.RestApi.Models.AddressBooks;

namespace MiniERP.RestApi.Endpoints.AddressBooks;

internal sealed class Create(IAddressRequestMapper<CreateAddressRequest> addressRequestMapper) : IEndpoint
{
    private readonly IAddressRequestMapper<CreateAddressRequest> _addressRequestMapper = addressRequestMapper
        ?? throw new ArgumentNullException(nameof(addressRequestMapper));

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/addresses", async (ISender sender, CreateAddressRequest request) => await CreateMethod(sender, request))
        .WithTags(Tags.Addresses);
    }

    private async Task<IResult> CreateMethod(ISender sender, CreateAddressRequest request)
    {
        var addressDto = _addressRequestMapper.MapToAddressDto(request);
        var result = await sender.Send(new CreateAddressCommand(addressDto));

        return result.IsSuccess
            ? Results.Created($"/addresses/{result.Value}", result.Value)
            : Results.BadRequest(result.Errors.Select(e => e.Message));

    }
}
