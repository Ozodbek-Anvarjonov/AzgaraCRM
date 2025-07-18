﻿using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Domain.Exceptions;
using AzgaraCRM.WebUI.Domain.Models;
using AzgaraCRM.WebUI.Extensions;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AzgaraCRM.WebUI.Services;

public class UserService(IUnitOfWork unitOfWork, IPasswordHasherService hasherService) : IUserService
{
    public async Task<IEnumerable<User>> GetAllAsync(
        PaginationParameters @params,
        SortingParameters sort,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var users = unitOfWork.Users.SelectAsQueryable();

        if (search is not null)
            users = users.Where(entity => entity.FirstName.ToLower().Contains(search.ToLower())
                || entity.LastName.ToLower().Contains(search.ToLower())
                || entity.Email.ToLower().Contains(search.ToLower()));

        users = users.Where(entity => !entity.IsDeleted).SortBy(sort);

        return await users.ToPaginate(@params).ToListAsync(cancellationToken);
    }

    public async Task<User> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.SelectAsync(entity => entity.Id == id && !entity.IsDeleted, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(User), id);

        return user;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var result = await unitOfWork.ExecuteInTransactionAsync<User>(async () =>
        {
            user.Password = await hasherService.HashPasswordAsync(user.Password);
            user.Role = UserRole.Admin;
            var entity = await unitOfWork.Users.AddAsync(user, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task<User> UpdateAsync(long id, User user, CancellationToken cancellationToken = default)
    {
        var existUser = await GetByIdAsync(id, cancellationToken);

        existUser.FirstName = user.FirstName;
        existUser.LastName = user.LastName;

        var result = await unitOfWork.ExecuteInTransactionAsync<User>(async () =>
        {
            var entity = await unitOfWork.Users.ModifyAsync(existUser, cancellationToken: cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        });

        return result;
    }

    public async Task<bool> EnableAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.SelectAsync(entity => entity.Id == id && entity.Role == UserRole.Admin && !entity.IsDeleted, isTracked: true, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(User), id);

        user.IsDisabled = false;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DisableAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.SelectAsync(entity => entity.Id == id && entity.Role == UserRole.Admin && !entity.IsDeleted, isTracked: true, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(User), id);

        user.IsDisabled = true;
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);

        if (user.Role == UserRole.Owner)
            throw new ArgumentIsNotValidException("The user in this role can not be deleted.");

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await unitOfWork.Users.RemoveAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        });
    }
}