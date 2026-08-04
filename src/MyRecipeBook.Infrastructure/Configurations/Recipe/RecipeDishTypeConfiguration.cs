using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations.Recipe;

public class RecipeDishTypeConfiguration : IEntityTypeConfiguration<RecipeDishType>
{
    public void Configure(EntityTypeBuilder<RecipeDishType> builder)
    {
        builder.ToTable("RecipeDishTypes");
        builder.Property(r => r.Type).HasConversion<string>();
        builder.Property(r => r.Id).ValueGeneratedNever();
    }
}