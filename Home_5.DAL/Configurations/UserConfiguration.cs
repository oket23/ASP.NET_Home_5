using Home_5.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Home_5.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.Property(x => x.BirthDate)
            .HasColumnType("datetime")
            .IsRequired();
        
        builder.HasMany(x => x.Subscriptions)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}