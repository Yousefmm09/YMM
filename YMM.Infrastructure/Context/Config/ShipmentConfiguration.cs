using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TrackingNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.TrackingNumber)
                .IsUnique();

            builder.Property(x => x.Carrier)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Preparing");

            builder.Property(x => x.CurrentLocation)
                .HasMaxLength(255);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Order)
                .WithOne(o => o.Shipment)
                .HasForeignKey<Shipment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.TrackingHistory)
                .WithOne(t => t.Shipment)
                .HasForeignKey(t => t.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
