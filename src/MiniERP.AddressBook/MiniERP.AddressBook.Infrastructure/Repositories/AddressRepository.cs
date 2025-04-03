using FluentResults;

using Microsoft.EntityFrameworkCore;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;

namespace MiniERP.AddressBook.Infrastructure.Repositories
{
    public class AddressRepository : IRepository<Address>
    {
        private readonly AddressBookContext _context;

        public AddressRepository(AddressBookContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<Address>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

            if (address == null)
            {
                return Result.Fail(ResultErrors.NotFound<Address>(id));
            }

            return Result.Ok(address);
        }

        public async Task<Result<IEnumerable<Address>>> GetAsync(CancellationToken cancellationToken)
        {
            var addresses = await _context.Addresses
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Ok(addresses.AsEnumerable());
        }

        public async Task<Result<int>> AddAsync(Address entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Address entity cannot be null");
            }

            try
            {
                _context.Addresses.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> UpdateAsync(Address entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Address entity cannot be null");
            }

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses.FindAsync(new object[] { id }, cancellationToken);
            if (address == null)
            {
                return Result.Fail("Address not found");
            }

            _context.Addresses.Remove(address);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Addresses.AnyAsync(a => a.Id == id, cancellationToken);
        }
    }
}
