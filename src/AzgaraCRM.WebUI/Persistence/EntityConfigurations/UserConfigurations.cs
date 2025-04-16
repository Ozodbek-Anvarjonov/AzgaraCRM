using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
using AzgaraCRM.WebUI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzgaraCRM.WebUI.Persistence.EntityConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasIndex(entity => entity.Email)
            .IsUnique();

        builder
            .HasData(new User
            {
                Id = 1,
                CreatedAt = default,
                FirstName = "system",
                LastName = "system",
                Email = "system@system",
                Password = new PasswordHasherService().HashPassword("system.developer"),
                Role = UserRole.Owner,
            });
    }
}