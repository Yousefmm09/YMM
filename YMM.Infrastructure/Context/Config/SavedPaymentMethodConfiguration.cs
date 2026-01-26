using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class SavedPaymentMethodConfiguration : IEntityTypeConfiguration<SavedPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<SavedPaymentMethod> builder)
        {
            builder.ToTable("SavedPaymentMethods");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CardHolderName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Last4Digits)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Brand)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.User)
                .WithMany(u => u.SavedPaymentMethods)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
