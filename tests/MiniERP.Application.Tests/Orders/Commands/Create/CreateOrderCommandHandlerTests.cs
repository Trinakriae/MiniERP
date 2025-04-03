using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Commands.Create;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Commands.Create
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IMapper<Order, OrderDto>> _mockOrderMapper;
        private readonly Mock<IValidator<CreateOrderCommand>> _mockValidator;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockOrderMapper = new Mock<IMapper<Order, OrderDto>>();
            _mockValidator = new Mock<IValidator<CreateOrderCommand>>();
            _handler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockOrderMapper.Object,
                _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenOrderIsValid()
        {
            // Arrange
            var orderDto = new OrderDto { Lines = [new OrderLineDto { ProductId = 1 }] };
            var order = new Order { Lines = [new OrderLine { ProductId = 1 }] };
            var command = new CreateOrderCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockOrderMapper.Setup(m => m.Map(orderDto)).Returns(order);
            _mockOrderRepository.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenOrderIsInvalid()
        {
            // Arrange
            var orderDto = new OrderDto { Lines = [new OrderLineDto { ProductId = 1 }] };
            var command = new CreateOrderCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult([new ValidationFailure("Lines", "Invalid")]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}
