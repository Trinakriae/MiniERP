using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.AddressBook.Infrastructure.Repositories;
using MiniERP.Application.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddAddressBookInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AddressBookDbConnection");

        services.AddDbContext<AddressBookContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IRepository<Address>, AddressRepository>();

        return services;
    }
}