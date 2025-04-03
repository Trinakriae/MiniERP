using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Enums;
using MiniERP.RestApi.Models.Orders;

namespace MiniERP.RestApi.Tests.Endpoints.Orders
{
    public class OrderEndpointsTests : TestBase
    {
        [Fact]
        public async Task CreateOrder_ShouldReturnCreated_WhenSuccessful()
        {
            // Arrange
            var createOrderRequest = new CreateOrderRequest(
                "123",
                1,
                DateTime.UtcNow,
                1,
                new DeliveryAddressRequest(null, "Street", "City", "State", "PostalCode", "Country"),
                new List<OrderLineRequest>
                {
                    new(null, 1, 1, 100)
                }
            );

            // Act
            var response = await Client.PostAsJsonAsync("/api/orders", createOrderRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseObject = await response.Content.ReadFromJsonAsync<int>();
            responseObject.Should().Be(4);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOk_WhenOrderExists()
        {
            // Act
            var response = await Client.GetAsync("/api/orders/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = await response.Content.ReadFromJsonAsync<OrderDto>();
            order.Should().NotBeNull();
            order.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetAllOrders_ShouldReturnOk_WhenOrdersExist()
        {
            // Act
            var response = await Client.GetAsync("/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var orders = await response.Content.ReadFromJsonAsync<List<OrderDto>>();
            orders.Should().NotBeNull();
            orders.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var updateOrderRequest = new UpdateOrderRequest(
                1,
                "123",
                OrderStatus.Pending,
                DateTime.UtcNow,
                1,
                new DeliveryAddressRequest(1, "Street", "City", "State", "PostalCode", "Country"),
                new List<OrderLineRequest>
                {
                    new OrderLineRequest(null, 1, 1, 100),
                    new OrderLineRequest(null, 1, 1, 100),
                }
            );

            // Act
            var updateResponse = await Client.PutAsJsonAsync("/api/orders/1", updateOrderRequest);
            var getResponse = await Client.GetAsync("/api/orders/1");


            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = await getResponse.Content.ReadFromJsonAsync<OrderDto>();
            order.Should().NotBeNull();
            order.Id.Should().Be(1);
            order.Lines.Should().HaveCount(2);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContent_WhenSuccessful()
        {
            // Act
            var response = await Client.DeleteAsync("/api/orders/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

