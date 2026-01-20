using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using YMM.Data.Entities;

namespace YMM.Infrastructure.Context.Config
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Country).HasMaxLength(100).IsRequired();
            builder.Property(x => x.City).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Street).HasMaxLength(200).IsRequired();
            builder.Property(x => x.PostalCode).HasMaxLength(20);

            builder.HasOne(x => x.User)
                .WithMany(u =>u.Adress)
                .HasForeignKey(x => x.UserId);
        }
    }

}
