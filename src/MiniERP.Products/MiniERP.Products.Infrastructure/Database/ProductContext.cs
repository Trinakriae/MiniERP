using Microsoft.EntityFrameworkCore;

using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database.Configurations;

namespace MiniERP.Products.Infrastructure.Database;

public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
{
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductContext).Assembly);
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}