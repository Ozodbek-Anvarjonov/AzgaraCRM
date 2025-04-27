using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Enums;
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
                Password = "$2a$08$7HliKibMVbwPXiNo1m.eBesSFa4t3wWcA6nR3Wmte76ErM9rIRBiW",
                Role = UserRole.Owner,
            });
    }
}