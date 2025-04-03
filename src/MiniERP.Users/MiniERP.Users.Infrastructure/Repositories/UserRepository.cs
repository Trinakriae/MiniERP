using FluentResults;

using Microsoft.EntityFrameworkCore;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database;

namespace MiniERP.Users.Infrastructure.Repositories
{
    public class UserRepository(UserContext context) : IRepository<User>
    {
        private readonly UserContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Result<User>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
            {
                return Result.Fail(ResultErrors.NotFound<User>(id));
            }
            return Result.Ok(user);
        }

        public async Task<Result<IEnumerable<User>>> GetAsync(CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Ok(users.AsEnumerable());
        }

        public async Task<Result<int>> AddAsync(User entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("User entity cannot be null");
            }

            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("User entity cannot be null");
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
            var user = await _context.Users.FindAsync([id], cancellationToken);
            if (user == null)
            {
                return Result.Fail("User not found");
            }

            _context.Users.Remove(user);

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
            return await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }
    }
}
