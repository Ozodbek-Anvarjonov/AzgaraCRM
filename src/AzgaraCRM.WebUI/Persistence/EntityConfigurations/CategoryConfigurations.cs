using AzgaraCRM.WebUI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzgaraCRM.WebUI.Persistence.EntityConfigurations;

public class CategoryConfigurations : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder
            .HasMany(entity =>entity.Foods)
            .WithOne(entity => entity.Category)
            .HasForeignKey(entity => entity.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}