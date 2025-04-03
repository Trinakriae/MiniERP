using Microsoft.EntityFrameworkCore;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database.Configurations;

namespace MiniERP.AddressBook.Infrastructure.Database;

public class AddressBookContext(DbContextOptions<AddressBookContext> options) : DbContext(options)
{
    public virtual DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressBookContext).Assembly);

        modelBuilder.ApplyConfiguration(new AddressConfiguration());
    }
}