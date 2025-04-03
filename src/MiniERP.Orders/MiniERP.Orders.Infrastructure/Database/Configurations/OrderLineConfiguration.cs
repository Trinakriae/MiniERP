using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Orders.Infrastructure.Database.Configurations;

public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.HasKey(ol => ol.Id);

        builder.Property(ol => ol.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}