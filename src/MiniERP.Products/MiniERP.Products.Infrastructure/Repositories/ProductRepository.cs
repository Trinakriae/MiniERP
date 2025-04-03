using FluentResults;

using Microsoft.EntityFrameworkCore;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;

namespace MiniERP.Products.Infrastructure.Repositories
{
    public class ProductRepository(ProductContext context) : IRepository<Product>
    {
        private readonly ProductContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Result<Product>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
            {
                return Result.Fail(ResultErrors.NotFound<Product>(id));
            }
            return Result.Ok(product);
        }

        public async Task<Result<IEnumerable<Product>>> GetAsync(CancellationToken cancellationToken)
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Ok(products.AsEnumerable());
        }

        public async Task<Result<int>> AddAsync(Product entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Product entity cannot be null");
            }

            try
            {
                _context.Products.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> UpdateAsync(Product entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Product entity cannot be null");
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
            var product = await _context.Products.FindAsync([id], cancellationToken);
            if (product == null)
            {
                return Result.Fail("Product not found");
            }

            _context.Products.Remove(product);

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
            return await _context.Products.AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}
