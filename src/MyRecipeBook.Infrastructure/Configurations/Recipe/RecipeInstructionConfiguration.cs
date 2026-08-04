using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations.Recipe;

public class RecipeInstructionConfiguration : IEntityTypeConfiguration<RecipeInstruction>
{
    public void Configure(EntityTypeBuilder<RecipeInstruction> builder)
    {
        builder.ToTable("RecipeInstructions");
        builder.Property(r => r.Description).HasMaxLength(512);
        builder.Property(r => r.Id).ValueGeneratedNever();
    }
}