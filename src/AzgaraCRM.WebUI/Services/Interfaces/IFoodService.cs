using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Models;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IFoodService
{
    Task<IEnumerable<Food>> GetAllAsync(
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Food>> GetByCategoryIdAsync(
        long id,
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default);

    Task<Food> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Food> CreateAsync(Food food, IFormFile? file, CancellationToken cancellationToken = default);

    Task<Food> UpdateAsync(long id, Food food, IFormFile? file, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default);
}