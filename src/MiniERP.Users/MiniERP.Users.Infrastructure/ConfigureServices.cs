using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MiniERP.Application.Abstractions;
using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database;
using MiniERP.Users.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddUsersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("UserDbConnection");

        services.AddDbContext<UserContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IRepository<User>, UserRepository>();

        return services;
    }
}