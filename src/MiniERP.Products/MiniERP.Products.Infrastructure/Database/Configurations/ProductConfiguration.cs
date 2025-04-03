using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MiniERP.Products.Domain.Entities;

namespace MiniERP.Products.Infrastructure.Database.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasOne<Category>()
           .WithMany()
           .HasForeignKey(p => p.CategoryId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}


