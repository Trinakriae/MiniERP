# MiniERP

## Overview

Welcome to **MiniERP**—the *mightiest* mini ERP system you'll ever encounter! Inside , you'll find services like Orders, Products, Users, and AddressBook, each performing with its own dedicated database. With MiniERP, managing your business is not just efficient—it's an adventure!

## Prerequisites

Before diving in, ensure your development environment is equipped with the following tools:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

For those using Visual Studio 2022, MiniERP is preconfigured to run with the Play button (F5). Simply open the solution, hit play, and watch the magic unfold.

## Getting Started

### 1. Clone the Repository

Begin by cloning the MiniERP repository to your local machine:

```bash
git clone https://github.com/Trinakriae/MiniERP.git
cd MiniERP
```

### 2. Build and Run with Docker Compose

Use Docker Compose to build and run the application. This will start the SQL Server container and the application container.

```bash
docker-compose up --build
```

### 3. Running the Application with .NET CLI

Prefer to run the application using the .NET CLI? No problem! Navigate to the project directory and execute:

```bash
cd src/MiniERP.RestApi
dotnet run
```

### 4. Access the Application

Once the containers are up and running, navigate to the following URLs to interact with MiniERP:

- API: [http://localhost:58360](http://localhost:58360)
- Swagger UI: [http://localhost:58360/swagger](http://localhost:58360/swagger)

In case you run with .NET CLI:

- API: [http://localhost:5237](http://localhost:5237)
- Swagger UI: [http://localhost:5237/swagger](http://localhost:5237/swagger)

You can modify these settings as explained in the [Configuration section](#7-configuration)

### 5. Apply Migrations

To ensure your databases are in tune with the latest schema, apply migrations for each context. The Program.cs file is configured to automatically apply any pending migrations at startup. However, if you prefer to apply migrations manually, you can do so using the following commands:

```bash
dotnet ef database update --context AddressBookContext --project src/MiniERP.AddressBook.Infrastructure --startup-project src/MiniERP.RestApi
dotnet ef database update --context OrderContext --project src/MiniERP.Orders.Infrastructure --startup-project src/MiniERP.RestApi
dotnet ef database update --context ProductContext --project src/MiniERP.Products.Infrastructure --startup-project src/MiniERP.RestApi
dotnet ef database update --context UserContext --project src/MiniERP.Users.Infrastructure --startup-project src/MiniERP.RestApi
```

### 6. Environment Variables

Sensitive information such as database connection strings should be managed using environment variables or Docker secrets. You can define these in an `.env` file or use Docker secrets.

Example `.env` file:

```ini
SA_PASSWORD=YourStrong!Passw0rd
ADDRESSBOOK_DB_CONNECTION=Server=sqlserver;Database=AddressBookDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
ORDER_DB_CONNECTION=Server=sqlserver;Database=OrderDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
PRODUCT_DB_CONNECTION=Server=sqlserver;Database=ProductDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
USER_DB_CONNECTION=Server=sqlserver;Database=UserDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;
```

### 7. Configuration

Ensure your `appsettings.json` and `launchSettings.json` files are configured to perfection.

Example `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "AddressBookDbConnection": "Server=sqlserver;Database=AddressBookDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;",
    "OrderDbConnection": "Server=sqlserver;Database=OrderDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;",
    "ProductDbConnection": "Server=sqlserver;Database=ProductDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;",
    "UserDbConnection": "Server=sqlserver;Database=UserDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
  }
}
```

Example `launchSettings.json`:

```json
{
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "MiniERP.RestApi": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ConnectionStrings__AddressBookDbConnection": "Server=(localdb)\\mssqllocaldb;Database=AddressBookDb;Trusted_Connection=True;MultipleActiveResultSets=true",
        "ConnectionStrings__OrderDbConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderDb;Trusted_Connection=True;MultipleActiveResultSets=true",
        "ConnectionStrings__ProductDbConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductDb;Trusted_Connection=True;MultipleActiveResultSets=true",
        "ConnectionStrings__UserDbConnection": "Server=(localdb)\\mssqllocaldb;Database=UserDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      }
    }
  }
}
```

## Contributing

Contributions are welcome! If you have ideas, suggestions, or improvements, please open an issue or submit a pull request. Let's make MiniERP even mightier together!

## License

This project is licensed under the MIT License. Feel free to use, modify, and distribute it as you see fit.
