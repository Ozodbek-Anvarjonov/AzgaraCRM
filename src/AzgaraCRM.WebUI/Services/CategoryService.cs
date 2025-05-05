using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Exceptions;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AzgaraCRM.WebUI.Services;

public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
        public async Task<IEnumerable<Category>> GetAllAsync(
        PaginationParameters @params,
            SortingParameters sort,
            string? search = null,
            CancellationToken cancellationToken = default)
        {
            var categories = unitOfWork.Categories.SelectAsQueryable();

            if (search is not null)
                categories = categories.Where(entity => entity.Name.ToLower().Contains(search.ToLower()));

            categories = categories
                .Where(entity => !entity.IsDeleted)
                .Include(entity => entity.Foods.Where(food => !food.IsDeleted))
                .SortBy(sort);

            return await categories.ToPaginate(@params).ToListAsync(cancellationToken);
        }

    public async Task<Category> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var categories = await unitOfWork.Categories.SelectAsync(entity => entity.Id == id && !entity.IsDeleted, includes: ["Foods"], cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Category), id);
        return categories;
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.ExecuteInTransactionAsync<Category>(async () =>
        {
            var entity = await unitOfWork.Categories.AddAsync(category, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task<Category> UpdateAsync(long id, Category category, CancellationToken cancellationToken = default)
    {
        var existCategory = await GetByIdAsync(id, cancellationToken);

        existCategory.Name = category.Name;
        existCategory.LastModified = category.LastModified;

        var result = await unitOfWork.ExecuteInTransactionAsync<Category>(async () =>
        {
            var entity = await unitOfWork.Categories.ModifyAsync(existCategory, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var category = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await unitOfWork.Categories.RemoveAsync(category, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        });
    }
}