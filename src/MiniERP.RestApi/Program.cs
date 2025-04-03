using System.Reflection;

using MiniERP.RestApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddOrdersInfrastructure(builder.Configuration)
    .AddProductsInfrastructure(builder.Configuration)
    .AddAddressBookInfrastructure(builder.Configuration)
    .AddUsersInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseSwaggerWithUi();
    app.ApplyMigrations();
}


app.UseHttpsRedirection();

app.Run();


namespace MiniERP.RestApi
{
    public partial class Program;
}