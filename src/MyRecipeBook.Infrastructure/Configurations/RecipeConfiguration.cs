using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        
        builder.Property(r => r.Title).HasMaxLength(256);
        builder.Property(r => r.Description).HasMaxLength(512);
    }
}