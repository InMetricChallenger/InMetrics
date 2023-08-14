using Domain.Common;

namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    TRepository GetRepository<TRepository, TEntity>()
        where TRepository : class, IBaseRepository<TEntity>
        where TEntity : BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}