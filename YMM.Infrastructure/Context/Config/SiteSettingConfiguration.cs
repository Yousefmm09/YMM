using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
    {
        public void Configure(EntityTypeBuilder<SiteSetting> builder)
        {
            builder.ToTable("SiteSettings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Key)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Key)
                .IsUnique();

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(255);

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
