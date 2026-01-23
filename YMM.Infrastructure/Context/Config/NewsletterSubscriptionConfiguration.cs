using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class NewsletterSubscriptionConfiguration : IEntityTypeConfiguration<NewsletterSubscription>
    {
        public void Configure(EntityTypeBuilder<NewsletterSubscription> builder)
        {
            builder.ToTable("NewsletterSubscriptions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.UnsubscribeToken)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.SubscribedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
