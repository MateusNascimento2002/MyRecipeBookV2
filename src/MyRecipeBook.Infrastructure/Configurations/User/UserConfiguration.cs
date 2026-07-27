using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRecipeBook.Infrastructure.Configurations.User;

public class UserConfiguration : IEntityTypeConfiguration<Domain.Entities.User>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.User> builder)
    {
        builder.ToTable("Users");
        
        builder.Property(r => r.Name).HasMaxLength(256);
        builder.Property(r => r.Email).HasMaxLength(256);
    }
}