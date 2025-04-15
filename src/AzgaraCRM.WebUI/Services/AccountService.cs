using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Exceptions;
using AzgaraCRM.WebUI.Persistence.UnitOfWork;
using AzgaraCRM.WebUI.Services.Interfaces;

namespace AzgaraCRM.WebUI.Services;

public class AccountService(
    IUnitOfWork unitOfWork,
    IPasswordHasherService passwordHasherService,
    ITokenGeneratorService tokenGeneratorService) : IAccountService
{
    public async Task<(User User, string Token)> SignInAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Users.SelectAsync(entity => entity.Email == userName && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(User), userName);

        if (!await passwordHasherService.VerifyPasswordAsync(exist.Password, password))
            throw new CustomException("User name or password is wrong.", 400);

        var token = await tokenGeneratorService.GenerateTokenAsync(exist, cancellationToken);

        return new(exist, token);
    }
}