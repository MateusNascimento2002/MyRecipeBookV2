using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.Property(r => r.Name).HasMaxLength(256);
        builder.Property(r => r.Email).HasMaxLength(256);
    }
}