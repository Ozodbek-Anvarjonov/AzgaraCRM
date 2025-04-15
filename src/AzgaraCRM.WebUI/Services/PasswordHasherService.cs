using AzgaraCRM.WebUI.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace AzgaraCRM.WebUI.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private const int _workFactor = 8;

    public Task<string> HashPasswordAsync(string password, CancellationToken cancellationToken = default) =>
        Task.FromResult(BC.HashPassword(password, workFactor: _workFactor));

    public string HashPassword(string password) =>
        BC.HashPassword(password, workFactor: _workFactor);

    public Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword, CancellationToken cancellationToken = default) =>
        Task.FromResult(BC.Verify(providedPassword, hashedPassword));

    public bool VerifyPassword(string hashedPassword, string providedPassword) =>
        BC.Verify(providedPassword, hashedPassword);
}