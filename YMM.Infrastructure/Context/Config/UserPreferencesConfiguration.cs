using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities.Identity;

namespace YMM.Infrastructure.Context.Config
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.ToTable("UserPreferences");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Language)
                .HasMaxLength(10)
                .HasDefaultValue("en");

            builder.Property(x => x.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("EGP");

            builder.HasOne(x => x.User)
                .WithOne(u => u.Preferences)
                .HasForeignKey<UserPreferences>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId)
                .IsUnique();
        }
    }
}
