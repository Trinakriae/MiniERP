using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MiniERP.Application.Abstractions;
using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddProductsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductDbConnection");

        services.AddDbContext<ProductContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IRepository<Product>, ProductRepository>();
        services.AddScoped<IRepository<Category>, CategoryRepository>();

        return services;
    }
}