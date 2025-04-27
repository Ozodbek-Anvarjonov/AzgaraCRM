using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Exceptions;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AzgaraCRM.WebUI.Services;

public class FoodService(IUnitOfWork unitOfWork, IAssetService assetService) : IFoodService
{
    public async Task<IEnumerable<Food>> GetAllAsync(
    PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var foods = unitOfWork.Foods.SelectAsQueryable(includes: ["Category"]);

        if (search is not null)
            foods = foods.Where(entity => entity.Name.ToLower().Contains(search.ToLower())
                || entity.Price.ToString().Contains(search)
                || entity.Left.ToString().Contains(search));

        foods = foods.Where(entity => !entity.IsDeleted).SortBy(sort);

        return await foods.ToPaginate(@params).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Food>> GetByCategoryIdAsync(
        long id,
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var foods = unitOfWork.Foods.SelectAsQueryable(entity => entity.CategoryId == id, includes: ["Category"]);

        if (search is not null)
            foods = foods.Where(entity => entity.Name.ToLower().Contains(search.ToLower())
                || entity.Price.ToString().Contains(search)
                || entity.Left.ToString().Contains(search));

        foods = foods.Where(entity => !entity.IsDeleted).SortBy(sort);

        return await foods.ToPaginate(@params).ToListAsync(cancellationToken);
    }

    public async Task<Food> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var food = await unitOfWork.Foods.SelectAsync(entity => entity.Id == id && !entity.IsDeleted, includes: ["Category"], cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Food), id);
        return food;
    }

    public async Task<Food> CreateAsync(Food food, IFormFile? file, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.ExecuteInTransactionAsync<Food>(async () =>
        {
            _ = await unitOfWork.Categories.SelectAsync(entity => entity.Id == food.CategoryId && !entity.IsDeleted)
                ?? throw new NotFoundException(nameof(Category), food.CategoryId);

            if (file is not null)
                food.Path = await assetService.UploadAsync(file, cancellationToken: cancellationToken);
            var entity = await unitOfWork.Foods.AddAsync(food, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task<Food> UpdateAsync(long id, Food food, IFormFile? file, CancellationToken cancellationToken = default)
    {
        var existFood = await GetByIdAsync(id, cancellationToken);

        existFood.Name = food.Name;
        existFood.Price = food.Price;
        existFood.Left = food.Left;
        if (file is not null)
            existFood.Path = await assetService.UploadAsync(file, cancellationToken: cancellationToken);

        var result = await unitOfWork.ExecuteInTransactionAsync<Food>(async () =>
        {
            var entity = await unitOfWork.Foods.ModifyAsync(existFood, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var food = await GetByIdAsync(id, cancellationToken);

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await unitOfWork.Foods.RemoveAsync(food, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        });
    }
}