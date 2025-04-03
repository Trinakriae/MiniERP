using FluentValidation;

using MiniERP.Application.Orders.Commands.Delete;

namespace MiniERP.Application.Orders.Validators
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("Order ID must be greater than zero.");
        }
    }
}
