using FluentResults;

using Microsoft.EntityFrameworkCore;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Infrastructure.Database;

namespace MiniERP.Orders.Infrastructure.Repositories
{
    public class OrderRepository(OrderContext context) : IRepository<Order>
    {
        private readonly OrderContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<Result<Order>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Lines)
                .Include(o => o.DeliveryAddress)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

            if (order == null)
            {
                return Result.Fail(ResultErrors.NotFound<Order>(id));
            }

            return Result.Ok(order);
        }

        public async Task<Result<IEnumerable<Order>>> GetAsync(CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Lines)
                .Include(o => o.DeliveryAddress)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Ok(orders.AsEnumerable());
        }

        public async Task<Result<int>> AddAsync(Order entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Order entity cannot be null");
            }

            try
            {
                _context.Orders.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Ok(entity.Id);
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionalError(ex));
            }
        }

        public async Task<Result> UpdateAsync(Order entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                return Result.Fail("Order entity cannot be null");
            }

            try
            {
                var existingOrder = await _context.Orders
                    .Include(o => o.DeliveryAddress)
                    .Include(o => o.Lines)
                    .FirstOrDefaultAsync(o => o.Id == entity.Id, cancellationToken);

                if (existingOrder == null)
                {
                    return Result.Fail("Order not found.");
                }

                _context.Entry(existingOrder).CurrentValues.SetValues(entity);

                entity.DeliveryAddress.OrderId = entity.Id;
                _context.Remove(existingOrder.DeliveryAddress);
                _context.Add(entity.DeliveryAddress);

                var addedLines = entity.Lines.Where(l => l.Id == default).ToList();
                foreach (var addedLine in addedLines)
                {
                    addedLine.OrderId = entity.Id;
                    _context.OrderLines.Add(addedLine);
                }

                var updatedLineIds = entity.Lines.Select(l => l.Id).ToList();
                var removedLines = existingOrder.Lines.Where(l => !updatedLineIds.Contains(l.Id)).ToList();
                foreach (var removedLine in removedLines)
                {
                    _context.OrderLines.Remove(removedLine);
                }

                var updatedLines = entity.Lines.Where(l => l.Id != default).ToList();
                foreach (var updatedLine in updatedLines)
                {
                    var existingLine = existingOrder.Lines.FirstOrDefault(l => l.Id == updatedLine.Id);
                    if (existingLine != null)
                    {
                        _context.Entry(existingLine).CurrentValues.SetValues(updatedLine);
                    }
                }

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
            var order = await _context.Orders.FindAsync([id], cancellationToken);
            if (order == null)
            {
                return Result.Fail("Order not found");
            }

            _context.Orders.Remove(order);

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
            return await _context.Orders.AnyAsync(o => o.Id == id, cancellationToken);
        }
    }
}
