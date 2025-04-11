using System.Linq.Expressions;

namespace AzgaraCRM.WebUI.Persistence.Repositories;

public interface IRepository<TEntity>
{
    IQueryable<TEntity> SelectAsQueryable(
        Expression<Func<TEntity, bool>>? expression = default,
        string[]? includes = null, 
        bool isTracked = false);

    Task<TEntity?> SelectAsync(Expression<Func<TEntity, bool>> expression,
        string[]? includes = null,
        bool isTracked = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> ModifyAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
}