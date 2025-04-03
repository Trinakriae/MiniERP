using FluentResults;

using Microsoft.EntityFrameworkCore;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;

namespace MiniERP.Products.Infrastructure.Repositories
{
    public class CategoryRepository(ProductContext context) : IRepository<Category>
    {
        private readonly ProductContext _context = context
            ?? throw new ArgumentNullException(nameof(context));

        public async Task<Result<Category>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category == null)
            {
                return Result.Fail(ResultErrors.NotFound<Category>(id));
            }
            return Result.Ok(category);
        }

        public async Task<Result<IEnumerable<Category>>> GetAsync(CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Ok(categories.AsEnumerable());
        }

        public async Task<Result<int>> AddAsync(Category entity, CancellationToken cancellationToken)
        {
            try
            {
                _context.Categories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> UpdateAsync(Category entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Category entity cannot be null");
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
            var category = await _context.Categories.FindAsync([id], cancellationToken);
            if (category == null)
            {
                return Result.Fail("Category not found");
            }

            try
            {
                _context.Categories.Remove(category);
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
            return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
        }
    }
}
