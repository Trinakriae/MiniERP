using FluentValidation;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Update;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Addresses.Validators
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Address> _addressRepository;

        public UpdateAddressCommandValidator(
            IRepository<User> userRepository,
            IRepository<Address> addressRepository)
        {
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
            _addressRepository = addressRepository
                ?? throw new ArgumentNullException(nameof(addressRepository));

            RuleFor(x => x.AddressDto).NotNull().WithMessage("Address must not be null.");

            When(x => x.AddressDto != null, () =>
            {
                RuleFor(x => x.AddressDto.Id).NotNull().WithMessage("Address ID must not be empty.");

                RuleFor(x => x.AddressDto.Id.Value)
                    .MustAsync(AddressExists)
                    .WithMessage("Address does not exist.")
                    .When(x => x.AddressDto.Id.HasValue);

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

        private async Task<bool> AddressExists(int userId, CancellationToken cancellationToken)
        {
            return await _addressRepository.ExistsAsync(userId, cancellationToken);
        }
    }
}


