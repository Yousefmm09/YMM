using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class ReviewHelpfulConfiguration : IEntityTypeConfiguration<ReviewHelpful>
    {
        public void Configure(EntityTypeBuilder<ReviewHelpful> builder)
        {
            builder.ToTable("ReviewHelpfuls");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.Review)
                .WithMany(r => r.HelpfulMarks)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(u => u.ReviewHelpfuls)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint: one helpful mark per user per review
            builder.HasIndex(x => new { x.ReviewId, x.UserId })
                .IsUnique();
        }
    }
}
