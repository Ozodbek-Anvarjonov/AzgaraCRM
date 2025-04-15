namespace AzgaraCRM.WebUI.Services.Interfaces;

public interface IPasswordHasherService
{
    Task<string> HashPasswordAsync(string password, CancellationToken cancellationToken = default);

    string HashPassword(string password);

    Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword, CancellationToken cancellationToken = default);

    bool VerifyPassword(string hashedPassword, string providedPassword);
}