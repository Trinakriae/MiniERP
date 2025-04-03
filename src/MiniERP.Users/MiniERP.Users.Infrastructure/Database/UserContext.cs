using Microsoft.EntityFrameworkCore;

using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database.Configurations;

namespace MiniERP.Users.Infrastructure.Database;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserContext).Assembly);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}








