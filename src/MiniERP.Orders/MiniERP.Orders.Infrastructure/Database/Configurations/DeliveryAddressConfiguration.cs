using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Orders.Infrastructure.Database.Configurations;

public class DeliveryAddressConfiguration : IEntityTypeConfiguration<DeliveryAddress>
{
    public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
    {
        builder.HasKey(d => d.Id);
    }
}