using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Orders.Commands.Update;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Commands.Update
{
    public class UpdateOrderCommandHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IMapper<Order, OrderDto>> _mockOrderMapper;
        private readonly Mock<IValidator<UpdateOrderCommand>> _mockValidator;
        private readonly UpdateOrderCommandHandler _handler;

        public UpdateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockOrderMapper = new Mock<IMapper<Order, OrderDto>>();
            _mockValidator = new Mock<IValidator<UpdateOrderCommand>>();
            _handler = new UpdateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockOrderMapper.Object,
                _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenOrderIsValid()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 1, Date = DateTime.UtcNow.AddMinutes(-1), DeliveryAddress = new DeliveryAddressDto(), User = new OrderUserDto { Id = 1 } };
            var order = new Order { Id = 1, Date = DateTime.UtcNow.AddMinutes(-1), DeliveryAddress = new DeliveryAddress(), UserId = 1 };
            var command = new UpdateOrderCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockOrderMapper.Setup(m => m.Map(orderDto)).Returns(order);
            _mockOrderRepository.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());
            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(order));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenOrderIsInvalid()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 1, Date = DateTime.UtcNow.AddMinutes(-1), DeliveryAddress = new DeliveryAddressDto(), User = new OrderUserDto { Id = 1 } };
            var command = new UpdateOrderCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("OrderDto.UserId", "User does not exist.")
                ]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenOrderIsNotFound()
        {
            // Arrange
            var orderDto = new OrderDto { Id = 1 };
            var command = new UpdateOrderCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockOrderRepository.Setup(r => r.GetByIdAsync(orderDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Order>(orderDto.Id.Value)));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OrderNotFoundException>();
        }
    }
}
