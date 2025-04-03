using FluentValidation.TestHelper;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Commands.Create;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Validators;
using MiniERP.Orders.Domain.Enums;
using MiniERP.Products.Domain.Entities;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Validators
{
    public class CreateOrderValidatorTests
    {
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly CreateOrderCommandValidator _validator;

        public CreateOrderValidatorTests()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _validator = new CreateOrderCommandValidator(_mockProductRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIsNull()
        {
            // Arrange
            CreateOrderCommand? command = new(null as OrderDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto).WithErrorMessage("Order must not be null.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenOrderLinesAreEmpty()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Lines = [],
                Date = DateTime.UtcNow.AddMinutes(-1),
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderDateIsInTheFuture()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Date = DateTime.UtcNow.AddDays(1),
                Lines = [new OrderLineDto { ProductId = 1 }],
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

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
                DeliveryAddress = null,
                Lines = [new OrderLineDto { ProductId = 1 }],
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

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
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow,
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor("OrderDto.Lines[0]").WithErrorMessage("One or more product IDs do not exist.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIdIsNotEmpty()
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
            var command = new CreateOrderCommand(orderDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.Id).WithErrorMessage("Order ID must be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderStatusIsInvalid()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Status = (OrderStatus)999,
                Lines = [new OrderLineDto { ProductId = 1 }],
                Date = DateTime.UtcNow,
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderDto.Status).WithErrorMessage("Invalid order status.");
        }
        [Fact]
        public async Task Should_NotHaveValidationError_WhenOrderIsValid()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                Lines = [new() { ProductId = 1 }],
                Date = DateTime.UtcNow.AddMinutes(-1),
                DeliveryAddress = new DeliveryAddressDto(),
                User = new OrderUserDto { Id = 1 },
            };
            var command = new CreateOrderCommand(orderDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
