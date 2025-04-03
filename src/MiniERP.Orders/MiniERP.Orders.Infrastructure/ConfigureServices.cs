using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MiniERP.Application.Abstractions;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Orders.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddOrdersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrderDbConnection");

        services.AddDbContext<OrderContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IRepository<Order>, OrderRepository>();

        return services;
    }
}