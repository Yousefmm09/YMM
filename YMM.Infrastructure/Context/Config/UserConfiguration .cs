using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities.Identity;

namespace YMM.Infrastructure.Context.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);


            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.SecurityStamp)
                .HasDefaultValueSql("sysdatetime()");
        }
    }
}
