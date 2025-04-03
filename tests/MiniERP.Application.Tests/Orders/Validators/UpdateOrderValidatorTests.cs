using FluentValidation.TestHelper;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Commands.Update;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Validators;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Products.Domain.Entities;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Validators
{
    public class UpdateOrderValidatorTests
    {
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly UpdateOrderCommandValidator _validator;

        public UpdateOrderValidatorTests()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _validator = new UpdateOrderCommandValidator(_mockProductRepository.Object, _mockUserRepository.Object, _mockOrderRepository.Object);
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIsNull()
        {
            // Arrange
            UpdateOrderCommand? command = new(null as OrderDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto).WithErrorMessage("Order must not be null.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIdIsEmpty()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = null,
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow.AddSeconds(-1),
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);
            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.Id).WithErrorMessage("Order ID must not be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow,
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.Id.Value).WithErrorMessage("Order does not exist.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderDateIsInTheFuture()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                Date = DateTime.UtcNow.AddDays(1),
                Lines = [new OrderLineDto { ProductId = 1 }],
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.Date).WithErrorMessage("Order date cannot be in the future.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenDeliveryAddressIsNull()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                DeliveryAddress = null,
                Lines = [new OrderLineDto { ProductId = 1 }],
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.DeliveryAddress).WithErrorMessage("Order must have a valid delivery address.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductDoesNotExist()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow,
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor("OrderDto.Lines[0]").WithErrorMessage("One or more product IDs do not exist.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenUserIdIsInvalid()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow,
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.User.Id).WithErrorMessage("User does not exist.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenOrderIsValid()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Id = 1,
                Lines = [new() { ProductId = 1 }],
                Date = DateTime.UtcNow.AddMinutes(-1),
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new UpdateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockOrderRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
