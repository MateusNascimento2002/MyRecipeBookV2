using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations.Recipe;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable("RecipeIngredients");
        builder.Property(r => r.Item).HasMaxLength(512);
    }
}