using Home_5.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Home_5.DAL.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(x => x.StartDate)
            .HasColumnType("datetime")
            .IsRequired();
        
        builder.Property(x => x.EndDate)
            .HasColumnType("datetime")
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.IsCanceled)
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}