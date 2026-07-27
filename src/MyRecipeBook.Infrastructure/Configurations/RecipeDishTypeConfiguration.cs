using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations;

public class RecipeDishTypeConfiguration : IEntityTypeConfiguration<RecipeDishType>
{
    public void Configure(EntityTypeBuilder<RecipeDishType> builder)
    {
        builder.ToTable("RecipeDishTypes");
    }
}