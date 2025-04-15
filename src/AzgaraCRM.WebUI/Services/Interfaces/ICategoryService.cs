using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Models;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync(
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default);

    Task<Category> UpdateAsync(long id, Category category, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);
}
