using DevIO.Business.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Logradouro)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Number)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(p => p.Cep)
                .IsRequired()
                .HasColumnType("varchar(8)");

            builder.Property(p => p.Complement)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(p => p.Neighborhood)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.City)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.State)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.ToTable("Address");

        }
    }
}
