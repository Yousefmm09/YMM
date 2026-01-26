using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class ShipmentTrackingConfiguration : IEntityTypeConfiguration<ShipmentTracking>
    {
        public void Configure(EntityTypeBuilder<ShipmentTracking> builder)
        {
            builder.ToTable("ShipmentTrackings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Location)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
