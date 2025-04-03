using MiniERP.RestApi.Abstractions.AddressBooks;
using MiniERP.RestApi.Abstractions.Orders;
using MiniERP.RestApi.Abstractions.Products;
using MiniERP.RestApi.Abstractions.Users;
using MiniERP.RestApi.ErrorHandling;
using MiniERP.RestApi.Mappers.AddressBooks;
using MiniERP.RestApi.Mappers.Orders;
using MiniERP.RestApi.Mappers.Products;
using MiniERP.RestApi.Mappers.Users;
using MiniERP.RestApi.Models.AddressBooks;
using MiniERP.RestApi.Models.Orders;
using MiniERP.RestApi.Models.Products;
using MiniERP.RestApi.Models.Users;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddTransient<IOrderRequestMapper<CreateOrderRequest>, CreateOrderRequestMapper>();
        services.AddTransient<IOrderRequestMapper<UpdateOrderRequest>, UpdateOrderRequestMapper>();
        services.AddTransient<IUserRequestMapper<CreateUserRequest>, CreateUserRequestMapper>();
        services.AddTransient<IUserRequestMapper<UpdateUserRequest>, UpdateUserRequestMapper>();
        services.AddTransient<IProductRequestMapper<CreateProductRequest>, CreateProductRequestMapper>();
        services.AddTransient<IProductRequestMapper<UpdateProductRequest>, UpdateProductRequestMapper>();
        services.AddTransient<IAddressRequestMapper<CreateAddressRequest>, CreateAddressRequestMapper>();
        services.AddTransient<IAddressRequestMapper<UpdateAddressRequest>, UpdateAddressRequestMapper>();

        return services;
    }
}
