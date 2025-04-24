using AzgaraCRM.WebUI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzgaraCRM.WebUI.Persistence.EntityConfigurations;

public class FoodConfigurations : IEntityTypeConfiguration<Food>
{
    public void Configure(EntityTypeBuilder<Food> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder
            .HasOne(entity => entity.Category)
            .WithMany(entity => entity.Foods)
            .HasForeignKey(entity => entity.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}