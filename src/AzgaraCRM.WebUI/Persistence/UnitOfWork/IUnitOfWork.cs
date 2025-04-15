using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Persistence.Repositories;

namespace AzgaraCRM.WebUI.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    
    IRepository<Category> Categories { get; }

    IRepository<Food> Foods { get; }

    Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}