using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using MiniERP.Application.Users.Dtos;
using MiniERP.RestApi.Models.Users;

namespace MiniERP.RestApi.Tests.Endpoints.Users
{
    public class UserEndpointsTests : TestBase
    {

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenSuccessful()
        {
            var createUserRequest = new CreateUserRequest(
                "John",
                "Doe",
                "john.doe@example.com",
                "1234567890"
            );

            // Act
            var response = await Client.PostAsJsonAsync("/api/users", createUserRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseObject = await response.Content.ReadFromJsonAsync<int>();
            responseObject.Should().Be(4);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnOk_WhenUserExists()
        {
            // Act
            var response = await Client.GetAsync("/api/users/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            user.Should().NotBeNull();
            user.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WhenUsersExist()
        {
            // Act
            var response = await Client.GetAsync("/api/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            users.Should().NotBeNull();
            users.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNoContent_WhenSuccessful()
        {
            var updateUserRequest = new UpdateUserRequest(
                1,
                "John",
                "Doe",
                "john.doe@example.com",
                "1234567890"
            );

            // Act
            var response = await Client.PutAsJsonAsync("/api/users/1", updateUserRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenSuccessful()
        {
            // Act
            var response = await Client.DeleteAsync("/api/users/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
