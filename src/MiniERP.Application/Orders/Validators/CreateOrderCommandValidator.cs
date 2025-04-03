using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Commands.Create;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Products.Domain.Entities;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Orders.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<User> _userRepository;

        public CreateOrderCommandValidator(
            IRepository<Product> productRepository,
            IRepository<User> userRepository)
        {
            _productRepository = productRepository
                ?? throw new ArgumentNullException(nameof(productRepository));
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));

            RuleFor(x => x.OrderDto).NotNull().WithMessage("Order must not be null.");

            When(x => x.OrderDto != null, () =>
            {
                RuleFor(x => x.OrderDto.Id).Null().WithMessage("Order ID must be empty.");
                RuleFor(x => x.OrderDto.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future.");
                RuleFor(x => x.OrderDto.DeliveryAddress).NotNull().WithMessage("Order must have a valid delivery address.");

                RuleForEach(x => x.OrderDto.Lines)
                    .MustAsync(ProductExists)
                    .WithMessage("One or more product IDs do not exist.")
                    .When(x => x.OrderDto.Lines.Count > 0);

                RuleForEach(x => x.OrderDto.Lines)
                     .Must(line => !line.Id.HasValue)
                     .WithMessage("Order line ID must be empty.")
                     .When(x => x.OrderDto.Lines.Count > 0);

                RuleFor(x => x.OrderDto.User).NotNull().WithMessage("User must not be empty.");
                RuleFor(x => x.OrderDto.User.Id).GreaterThan(0).WithMessage("User ID must be greater than zero.");
                RuleFor(x => x.OrderDto.User.Id)
                    .MustAsync(UserExists)
                    .WithMessage("User does not exist.");

                RuleFor(x => x.OrderDto.Status).IsInEnum().WithMessage("Invalid order status.");
            });
        }

        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsAsync(userId, cancellationToken);
        }

        private async Task<bool> ProductExists(OrderLineDto line, CancellationToken cancellationToken)
        {
            return await _productRepository.ExistsAsync(line.ProductId, cancellationToken);
        }
    }
}
