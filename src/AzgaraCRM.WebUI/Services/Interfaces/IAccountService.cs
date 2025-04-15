using AzgaraCRM.WebUI.Domain.Entities;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IAccountService
{
    public Task<(User User, string Token)> SignInAsync(string userName, string password, CancellationToken cancellationToken = default);
}