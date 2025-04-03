using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Create;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Addresses.Validators
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        private readonly IRepository<User> _userRepository;

        public CreateAddressCommandValidator(IRepository<User> userRepository)
        {
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));

            RuleFor(x => x.AddressDto).NotNull().WithMessage("Address must not be null.");

            When(x => x.AddressDto != null, () =>
            {
                RuleFor(x => x.AddressDto.Id).Null().WithMessage("Address ID must be empty.");
                RuleFor(x => x.AddressDto.Street).NotEmpty().WithMessage("Street must not be empty.");
                RuleFor(x => x.AddressDto.City).NotEmpty().WithMessage("City must not be empty.");
                RuleFor(x => x.AddressDto.State).NotEmpty().WithMessage("State must not be empty.");
                RuleFor(x => x.AddressDto.PostalCode).NotEmpty().WithMessage("Postal code must not be empty.");
                RuleFor(x => x.AddressDto.Country).NotEmpty().WithMessage("Country must not be empty.");

                RuleFor(x => x.AddressDto.User).NotNull().WithMessage("User must not be null.");
                RuleFor(x => x.AddressDto.User.Id).GreaterThan(0).WithMessage("User ID must be greater than zero.");
                RuleFor(x => x.AddressDto.User.Id)
                    .MustAsync(UserExists)
                    .WithMessage("User does not exist.");


            });
        }

        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.ExistsAsync(userId, cancellationToken);
        }
    }
}


