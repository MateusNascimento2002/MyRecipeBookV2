using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyRecipeBook.Infrastructure.Configurations.VerificationCode;

public class VerificationCodeConfiguration: IEntityTypeConfiguration<Domain.Entities.VerificationCode>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.VerificationCode> builder)
    {
        builder.ToTable("VerificationCodes");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(v => v.CreatedAt).IsRequired();
        builder.Property(v => v.Code).HasMaxLength(6).IsRequired();
        builder.Property(v => v.Type).HasMaxLength(50).IsRequired().HasConversion<string>();
        builder.HasOne<Domain.Entities.User>()
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}