using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Products.Commands.Delete;
using MiniERP.Application.Users.Commands.Delete;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Commands.Delete
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<IValidator<DeleteProductCommand>> _validatorMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _validatorMock = new Mock<IValidator<DeleteProductCommand>>();
            _handler = new DeleteProductCommandHandler(_productRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenProductIsDeleted()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _productRepositoryMock.Setup(r => r.DeleteAsync(command.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());
            _productRepositoryMock.Setup(r => r.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(new Product { Id = command.ProductId }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            var validationResult = new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure("ProductId", "Validation error") });
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Validation error");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
        {
            // Arrange
            var command = new DeleteProductCommand(1);
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _productRepositoryMock.Setup(r => r.DeleteAsync(command.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail("Repository error"));
            _productRepositoryMock.Setup(r => r.GetByIdAsync(command.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(new Product { Id = command.ProductId }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(result.Errors, e => e.Message == "Repository error");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductIsNotFound()
        {
            // Arrange
            var productId = 1;
            var command = new DeleteProductCommand(productId);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Product>(1)));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}
