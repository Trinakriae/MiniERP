using Microsoft.EntityFrameworkCore;

using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Users.Infrastructure.Database;

namespace MiniERP.RestApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var contexts = new[]
        {
            typeof(AddressBookContext),
            typeof(OrderContext),
            typeof(ProductContext),
            typeof(UserContext)
        };

        foreach (var contextType in contexts)
        {
            var context = (DbContext)services.GetRequiredService(contextType);
            context.Database.Migrate();
        }
    }
}
