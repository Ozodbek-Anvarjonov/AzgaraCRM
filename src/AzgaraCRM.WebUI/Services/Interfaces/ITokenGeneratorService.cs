using AzgaraCRM.WebUI.Domain.Entities;

namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface ITokenGeneratorService
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}