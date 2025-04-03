using System.Reflection;

using FluentValidation;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Create;
using MiniERP.Application.Addresses.Commands.Delete;
using MiniERP.Application.Addresses.Commands.Update;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Mappers;
using MiniERP.Application.Addresses.Queries.GetById;
using MiniERP.Application.Addresses.Validators;
using MiniERP.Application.Orders.Commands.Create;
using MiniERP.Application.Orders.Commands.Delete;
using MiniERP.Application.Orders.Commands.Update;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Mappers;
using MiniERP.Application.Orders.Queries.GetById;
using MiniERP.Application.Orders.Validators;
using MiniERP.Application.Products.Commands.Create;
using MiniERP.Application.Products.Commands.Delete;
using MiniERP.Application.Products.Commands.Update;
using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Mappers;
using MiniERP.Application.Products.Queries.GetById;
using MiniERP.Application.Products.Validators;
using MiniERP.Application.Users.Commands.Create;
using MiniERP.Application.Users.Commands.Delete;
using MiniERP.Application.Users.Commands.Update;
using MiniERP.Application.Users.Dtos;
using MiniERP.Application.Users.Mappers;
using MiniERP.Application.Users.Queries.GetById;
using MiniERP.Application.Users.Validators;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Products.Domain.Entities;
using MiniERP.Users.Domain.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        services.AddScoped<IValidator<UpdateOrderCommand>, UpdateOrderCommandValidator>();
        services.AddScoped<IValidator<DeleteOrderCommand>, DeleteOrderCommandValidator>();
        services.AddScoped<IValidator<GetOrderByIdQuery>, GetOrderByIdQueryValidator>();

        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IValidator<UpdateUserCommand>, UpdateUserCommandValidator>();
        services.AddScoped<IValidator<DeleteUserCommand>, DeleteUserCommandValidator>();
        services.AddScoped<IValidator<GetUserByIdQuery>, GetUserByIdQueryValidator>();

        services.AddScoped<IValidator<CreateAddressCommand>, CreateAddressCommandValidator>();
        services.AddScoped<IValidator<UpdateAddressCommand>, UpdateAddressCommandValidator>();
        services.AddScoped<IValidator<DeleteAddressCommand>, DeleteAddressCommandValidator>();
        services.AddScoped<IValidator<GetAddressByIdQuery>, GetAddressByIdQueryValidator>();

        services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddScoped<IValidator<DeleteProductCommand>, DeleteProductCommandValidator>();
        services.AddScoped<IValidator<GetProductByIdQuery>, GetProductByIdQueryValidator>();

        services.AddScoped<IMapper<Order, OrderDto>, OrderMapper>();
        services.AddScoped<IMapper<OrderLine, OrderLineDto>, OrderLineMapper>();
        services.AddScoped<IMapper<DeliveryAddress, DeliveryAddressDto>, DeliveryAddressMapper>();
        services.AddScoped<IMapper<User, OrderUserDto>, UserToOrderUserDtoMapper>();

        services.AddScoped<IMapper<User, UserDto>, UserMapper>();

        services.AddScoped<IMapper<Address, AddressDto>, AddressMapper>();
        services.AddScoped<IMapper<User, AddressUserDto>, UserToAddressUserDtoMapper>();

        services.AddScoped<IMapper<Product, ProductDto>, ProductMapper>();
        services.AddScoped<IMapper<Category, CategoryDto>, CategoryMapper>();

        return services;
    }
}
