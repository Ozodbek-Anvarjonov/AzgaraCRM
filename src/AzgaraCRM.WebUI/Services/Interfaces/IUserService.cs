using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Models;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync(
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<User> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User> UpdateAsync(long id, User user, CancellationToken cancellationToken = default);

    Task<bool> EnableAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> DisableAsync(long id, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);
}