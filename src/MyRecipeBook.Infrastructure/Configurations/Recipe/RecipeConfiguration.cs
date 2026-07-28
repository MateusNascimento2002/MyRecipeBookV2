using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DomainRecipe = MyRecipeBook.Domain.Entities.Recipe;

namespace MyRecipeBook.Infrastructure.Configurations.Recipe;

public class RecipeConfiguration : IEntityTypeConfiguration<DomainRecipe>
{
    public void Configure(EntityTypeBuilder<DomainRecipe> builder)
    {
        builder.ToTable("Recipes");

        builder.Property(r => r.Title).HasMaxLength(256);
        builder.Property(r => r.Description).HasMaxLength(512);
        builder.Property(r => r.CookTime).HasConversion<string>();
        
        builder.HasOne<Domain.Entities.User>()
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
