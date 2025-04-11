using AzgaraCRM.WebUI.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AzgaraCRM.WebUI.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<TEntity> _set;

    public Repository(AppDbContext context)
    {
        _dbContext = context;
        _set = context.Set<TEntity>();
    }

    public IQueryable<TEntity> SelectAsQueryable(
        Expression<Func<TEntity, bool>>? expression = null,
        string[]? includes = null,
        bool isTracked = false)
    {
        var query = expression is not null ? _set.Where(expression) : _set;

        if (includes is not null)
            query = Include(query, includes);

        if (!isTracked)
            query = query.AsNoTracking();

        return query;
    }

    public async Task<TEntity?> SelectAsync(
        Expression<Func<TEntity, bool>> expression,
        string[]? includes = null,
        bool isTracked = false,
        CancellationToken cancellationToken = default)
    {
        var query = _set.Where(expression);

        if (includes is not null)
            query = Include(query, includes);

        if (!isTracked)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = await _set.AddAsync(entity, cancellationToken);

        return entry.Entity;
    }

    public Task<TEntity> ModifyAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entry = _set.Update(entity);

        return Task.FromResult(entry.Entity);
    }

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _ = _set.Remove(entity);

        return Task.CompletedTask;
    }

    private IQueryable<TEntity> Include(IQueryable<TEntity> source, string[] includes)
    {
        foreach (var include in includes)
            source = source.Include(include);

        return source;
    }
}