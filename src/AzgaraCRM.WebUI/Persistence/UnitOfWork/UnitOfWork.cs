using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Persistence.DataContexts;
using AzgaraCRM.WebUI.Persistence.Repositories;

namespace AzgaraCRM.WebUI.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext context, IRepository<User> users, IRepository<Category> categories, IRepository<Food> foods)
    {
        _dbContext = context;

        Users = users;
        Categories = categories;
        Foods = foods;
    }

    #region
    public IRepository<User> Users { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Food> Foods { get; }
    #endregion

    #region
    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);

        try
        {
            await action();
            await CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await action();
            await CommitTransactionAsync(cancellationToken);

            return result;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
}
