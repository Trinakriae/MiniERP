using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Infrastructure.Database.Configurations;

namespace MiniERP.Orders.Infrastructure.Database;

public class OrderContext(DbContextOptions<OrderContext> options) : DbContext(options)
{
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderLine> OrderLines { get; set; }
    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderContext).Assembly);
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderLineConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryAddressConfiguration());

    }
}