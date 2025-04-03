using FluentValidation;

using MiniERP.Application.Users.Commands.Delete;

namespace MiniERP.Application.Users.Validators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be greater than zero.");
        }
    }
}
