namespace MiniERP.Application.Errors;

public static class ErrorCodes
{
    public static class Order
    {
        public const string NotFound = "ORDER_NOT_FOUND";
    }

    public static class Category
    {
        public const string NotFound = "CATEGORY_NOT_FOUND";
    }

    public static class Product
    {
        public const string NotFound = "PRODUCT_NOT_FOUND";
    }

    public static class User
    {
        public const string NotFound = "USER_NOT_FOUND";
    }

    public static class Address
    {
        public const string NotFound = "ADDRESS_NOT_FOUND";
    }
}
