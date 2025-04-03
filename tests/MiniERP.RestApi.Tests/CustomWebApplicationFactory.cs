using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Users.Infrastructure.Database;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    // Hold connections so they stay open for the test lifetime
    private DbConnection? _orderConnection;
    private DbConnection? _userConnection;
    private DbConnection? _productConnection;
    private DbConnection? _addressBookConnection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // Remove existing DbContexts
            RemoveDbContext<OrderContext>(services);
            RemoveDbContext<UserContext>(services);
            RemoveDbContext<ProductContext>(services);
            RemoveDbContext<AddressBookContext>(services);

            RemoveDbContextConfiguration<OrderContext>(services);
            RemoveDbContextConfiguration<UserContext>(services);
            RemoveDbContextConfiguration<ProductContext>(services);
            RemoveDbContextConfiguration<AddressBookContext>(services);

            // Add individual open connections per context
            _orderConnection = CreateInMemoryConnection("OrderTestDb");
            _userConnection = CreateInMemoryConnection("UserTestDb");
            _productConnection = CreateInMemoryConnection("ProductTestDb");
            _addressBookConnection = CreateInMemoryConnection("AddressBookTestDb");

            services.AddSingleton(_orderConnection);
            services.AddSingleton(_userConnection);
            services.AddSingleton(_productConnection);
            services.AddSingleton(_addressBookConnection);

            // Register each DbContext
            services.AddDbContext<OrderContext>((sp, options) =>
                options.UseSqlite(_orderConnection!));

            services.AddDbContext<UserContext>((sp, options) =>
                options.UseSqlite(_userConnection!));

            services.AddDbContext<ProductContext>((sp, options) =>
                options.UseSqlite(_productConnection!));

            services.AddDbContext<AddressBookContext>((sp, options) =>
                options.UseSqlite(_addressBookConnection!));

            // Build provider, ensure schema, seed data
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scoped = scope.ServiceProvider;

            var orderDb = scoped.GetRequiredService<OrderContext>();
            var userDb = scoped.GetRequiredService<UserContext>();
            var productDb = scoped.GetRequiredService<ProductContext>();
            var addressDb = scoped.GetRequiredService<AddressBookContext>();

            orderDb.Database.EnsureCreated();
            userDb.Database.EnsureCreated();
            productDb.Database.EnsureCreated();
            addressDb.Database.EnsureCreated();
        });
    }

    private static DbConnection CreateInMemoryConnection(string name)
    {
        var connectionString = $"Filename={name};Mode=Memory;";
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        return connection;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _orderConnection?.Dispose();
        _userConnection?.Dispose();
        _productConnection?.Dispose();
        _addressBookConnection?.Dispose();
    }

    private void RemoveDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.RemoveAll(typeof(DbContextOptions<TContext>));
    }

    private void RemoveDbContextConfiguration<TContext>(IServiceCollection services) where TContext : DbContext
    {
        services.RemoveAll(typeof(IDbContextOptionsConfiguration<TContext>));
    }
}
