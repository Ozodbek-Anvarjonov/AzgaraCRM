using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Persistence.DataContexts;
using AzgaraCRM.WebUI.Services.Interfaces;

namespace AzgaraCRM.WebUI.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if(context.Users.Any())
                return;

            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasherService>();
            var user = new User
            {
                CreatedAt = default,
                FirstName = "system",
                LastName = "system",
                Email = "system@system",
                Password = hasher.HashPassword("system.developer"),
                Role = UserRole.Owner,
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}