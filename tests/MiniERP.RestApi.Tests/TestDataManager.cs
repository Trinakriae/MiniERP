using Microsoft.Extensions.DependencyInjection;

using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Users.Infrastructure.Database;

namespace MiniERP.RestApi.Tests
{
    public static class TestDataManager
    {
        public static void ResetAll(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scoped = scope.ServiceProvider;

            ClearAndSeedAddressBook(scoped);
            ClearAndSeedUsers(scoped);
            ClearAndSeedOrders(scoped);
            ClearAndSeedProducts(scoped);
        }

        private static void ClearAndSeedAddressBook(IServiceProvider services)
        {
            var db = services.GetRequiredService<AddressBookContext>();
            db.Addresses.RemoveRange(db.Addresses);
            db.SaveChanges();
            Seeding.InitializeAddressTestDb(db);
        }

        private static void ClearAndSeedUsers(IServiceProvider services)
        {
            var db = services.GetRequiredService<UserContext>();
            db.Users.RemoveRange(db.Users);
            db.SaveChanges();
            Seeding.InitializeUserTestDb(db);
        }

        private static void ClearAndSeedOrders(IServiceProvider services)
        {
            var db = services.GetRequiredService<OrderContext>();
            db.Orders.RemoveRange(db.Orders);
            db.SaveChanges();
            Seeding.InitializeOrderTestDb(db);
        }

        private static void ClearAndSeedProducts(IServiceProvider services)
        {
            var db = services.GetRequiredService<ProductContext>();
            db.Products.RemoveRange(db.Products);
            db.Categories.RemoveRange(db.Categories);
            db.SaveChanges();
            Seeding.InitializeProductTestDb(db);
        }
    }
}