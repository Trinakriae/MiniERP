using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Orders.Infrastructure.Database.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .HasConversion<int>();

        builder.HasMany(o => o.Lines)
            .WithOne()
            .HasForeignKey(ol => ol.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.DeliveryAddress)
            .WithOne()
            .HasForeignKey<DeliveryAddress>(nameof(DeliveryAddress.OrderId))
            .OnDelete(DeleteBehavior.Cascade);
    }
}