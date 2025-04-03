using FluentResults;

namespace MiniERP.Application.Abstractions
{
    public interface IRepository<T>
    {
        Task<Result<T>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<T>>> GetAsync(CancellationToken cancellationToken);
        Task<Result<int>> AddAsync(T entity, CancellationToken cancellationToken);
        Task<Result> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    }
}
