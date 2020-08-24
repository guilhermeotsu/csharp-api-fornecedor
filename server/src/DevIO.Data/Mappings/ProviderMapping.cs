using DevIO.Business.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Data.Mappings
{
    public class ProviderMapping : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(15)");

            
            // Relationship 1 : 1 => Provider : Address
            builder.HasOne(p => p.Address)
                .WithOne(e => e.Provider);

            // Relationship 1 : N => Provider : Producs
            builder.HasMany(x => x.Products)
                .WithOne(p => p.Provider)
                .HasForeignKey(p => p.ProviderId);

            builder.ToTable("Providers");
        }
    }
}
