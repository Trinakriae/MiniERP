using FluentResults;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Errors;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Products.Domain.Entities;
using MiniERP.Users.Domain.Entities;


namespace MiniERP.Application.Common.Errors;

public static class ResultErrors
{
    public static Error NotFound<T>(object id)
    {
        string errorCode = typeof(T).Name switch
        {
            nameof(Order) => ErrorCodes.Order.NotFound,
            nameof(Category) => ErrorCodes.Category.NotFound,
            nameof(Product) => ErrorCodes.Product.NotFound,
            nameof(User) => ErrorCodes.User.NotFound,
            nameof(Address) => ErrorCodes.Address.NotFound,
            _ => "NOT_FOUND"
        };

        return new Error($"{typeof(T).Name} with ID {id} was not found")
            .WithMetadata("ErrorCode", errorCode)
            .WithMetadata("Id", id);
    }
}