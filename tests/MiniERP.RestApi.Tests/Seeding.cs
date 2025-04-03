using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Domain.Enums;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database;

namespace MiniERP.RestApi.Tests
{
    public class Seeding
    {
        public static void InitializeOrderTestDb(OrderContext db)
        {
            var orders = GetTestOrders();
            db.Orders.AddRange(orders);
            db.SaveChanges();
        }

        public static void InitializeProductTestDb(ProductContext db)
        {
            var testCategories = GetTestCategories();
            db.Categories.AddRange(testCategories);

            var products = GetTestProducts();
            db.Products.AddRange(products);
            db.SaveChanges();
        }

        public static void InitializeAddressTestDb(AddressBookContext db)
        {
            var addresses = GetTestAddresses();
            db.Addresses.AddRange(addresses);
            db.SaveChanges();
        }

        public static void InitializeUserTestDb(UserContext db)
        {
            var users = GetTestUsers();
            db.Users.AddRange(users);
            db.SaveChanges();
        }


        private static IEnumerable<Order> GetTestOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    Id = 1,
                    OrderNumber = "123",
                    UserId = 1,
                    Date = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    DeliveryAddress = new DeliveryAddress
                    {
                        Street = "Street",
                        City = "City",
                        State = "State",
                        PostalCode = "PostalCode",
                        Country = "Country"
                    },
                    Lines = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            ProductId = 1,
                            Quantity = 1,
                            Price = 100
                        }
                    }
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = "124",
                    UserId = 2,
                    Date = DateTime.UtcNow,
                    Status = OrderStatus.Delivered,
                    DeliveryAddress = new DeliveryAddress
                    {
                        Street = "Another Street",
                        City = "Another City",
                        State = "Another State",
                        PostalCode = "Another PostalCode",
                        Country = "Another Country"
                    },
                    Lines = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            ProductId = 2,
                            Quantity = 2,
                            Price = 200
                        },
                        new OrderLine
                        {
                            ProductId = 3,
                            Quantity = 3,
                            Price = 300
                        }
                    }
                },
                new Order
                {
                    Id = 3,
                    OrderNumber = "125",
                    UserId = 3,
                    Date = DateTime.UtcNow,
                    Status = OrderStatus.Shipped,
                    DeliveryAddress = new DeliveryAddress
                    {
                        Street = "Third Street",
                        City = "Third City",
                        State = "Third State",
                        PostalCode = "Third PostalCode",
                        Country = "Third Country"
                    },
                    Lines = new List<OrderLine>
                    {
                        new OrderLine
                        {
                            ProductId = 4,
                            Quantity = 4,
                            Price = 400
                        },
                        new OrderLine
                        {
                            ProductId = 5,
                            Quantity = 5,
                            Price = 500
                        }
                    }
                }
            };
        }

        private static IEnumerable<Product> GetTestProducts()
        {

            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Test Product 1",
                    Description = "Test Description 1",
                    UnitPrice = 10.0m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Test Product 2",
                    Description = "Test Description 2",
                    UnitPrice = 20.0m,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Test Product 3",
                    Description = "Test Description 3",
                    UnitPrice = 30.0m,
                    CategoryId = 3
                }
            };
        }

        private static List<Category> GetTestCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Test Category 1"
                },
                new Category
                {
                    Id = 2,
                    Name = "Test Category 2"
                },
                new Category
                {
                    Id = 3,
                    Name = "Test Category 3"
                }
            };
        }

        private static IEnumerable<Address> GetTestAddresses()
        {
            return new List<Address>
            {
                new Address
                {
                    Id = 1,
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "Anystate",
                    PostalCode = "12345",
                    Country = "USA",
                    UserId = 1
                },
                new Address
                {
                    Id = 2,
                    Street = "456 Elm St",
                    City = "Othertown",
                    State = "Otherstate",
                    PostalCode = "67890",
                    Country = "USA",
                    UserId = 2
                },
                new Address
                {
                    Id = 3,
                    Street = "789 Oak St",
                    City = "Sometown",
                    State = "Somestate",
                    PostalCode = "11223",
                    Country = "USA",
                    UserId = 3
                }
            };
        }

        private static IEnumerable<User> GetTestUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "123-456-7890"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "098-765-4321"
                },
                new User
                {
                    Id = 3,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    PhoneNumber = "555-555-5555"
                }
            };
        }
    }
}
