using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using MiniERP.Application.Addresses.Dtos;
using MiniERP.RestApi.Models.AddressBooks;

namespace MiniERP.RestApi.Tests.Endpoints.AddressBooks
{
    public class AddressEndpointsTests : TestBase
    {

        [Fact]
        public async Task CreateAddress_ShouldReturnCreated_WhenSuccessful()
        {
            var createAddressRequest = new CreateAddressRequest(
                "123 Main St",
                "Anytown",
                "Anystate",
                "12345",
                "USA",
                1
            );

            // Act
            var response = await Client.PostAsJsonAsync("/api/addresses", createAddressRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseObject = await response.Content.ReadFromJsonAsync<int>();
            responseObject.Should().Be(4);
        }

        [Fact]
        public async Task GetAddressById_ShouldReturnOk_WhenAddressExists()
        {

            // Act
            var response = await Client.GetAsync("/api/addresses/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var address = await response.Content.ReadFromJsonAsync<AddressDto>();
            address.Should().NotBeNull();
            address.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetAllAddresses_ShouldReturnOk_WhenAddressesExist()
        {
            // Act
            var response = await Client.GetAsync("/api/addresses");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var addresses = await response.Content.ReadFromJsonAsync<List<AddressDto>>();
            addresses.Should().NotBeNull();
            addresses.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAddress_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var updateAddressRequest = new UpdateAddressRequest(
                1,
                "123 Main St",
                "Anytown",
                "Anystate",
                "12345",
                "USA",
                2
            );

            // Act
            var response = await Client.PutAsJsonAsync("/api/addresses/1", updateAddressRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAddress_ShouldReturnNoContent_WhenSuccessful()
        {
            // Act
            var response = await Client.DeleteAsync("/api/addresses/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

