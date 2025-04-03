using FluentValidation;

using MiniERP.Application.Users.Commands.Create;

namespace MiniERP.Application.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserDto).NotNull().WithMessage("User must not be null.");

            When(x => x.UserDto != null, () =>
            {
                RuleFor(x => x.UserDto.Id).Null().WithMessage("User ID must be empty.");
                RuleFor(x => x.UserDto.FirstName).NotEmpty().WithMessage("First name must not be empty.");
                RuleFor(x => x.UserDto.LastName).NotEmpty().WithMessage("Last name must not be empty.");
                RuleFor(x => x.UserDto.Email).NotEmpty().WithMessage("Email must not be empty.")
                                             .EmailAddress().WithMessage("Email must be valid.");
                RuleFor(x => x.UserDto.PhoneNumber).NotEmpty().WithMessage("Phone number must not be empty.");
            });
        }
    }
}
