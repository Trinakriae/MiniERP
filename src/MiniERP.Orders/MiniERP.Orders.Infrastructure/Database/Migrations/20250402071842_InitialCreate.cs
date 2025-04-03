using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Orders table
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type = 'U')
                BEGIN
                    CREATE TABLE [dbo].[Orders] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [OrderNumber] NVARCHAR(MAX) NOT NULL,
                        [Status] INT NOT NULL,
                        [Date] DATETIME2 NOT NULL,
                        [UserId] INT NOT NULL,
                        CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END
            ");

            // Create DeliveryAddresses table
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeliveryAddresses]') AND type = 'U')
                BEGIN
                    CREATE TABLE [dbo].[DeliveryAddresses] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [Street] NVARCHAR(MAX) NOT NULL,
                        [City] NVARCHAR(MAX) NOT NULL,
                        [State] NVARCHAR(MAX) NOT NULL,
                        [PostalCode] NVARCHAR(MAX) NOT NULL,
                        [Country] NVARCHAR(MAX) NOT NULL,
                        [OrderId] INT NOT NULL,
                        CONSTRAINT [PK_DeliveryAddresses] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_DeliveryAddresses_Orders_OrderId] 
                            FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders]([Id]) 
                            ON DELETE CASCADE
                    );
                END
            ");

            // Create OrderLines table
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderLines]') AND type = 'U')
                BEGIN
                    CREATE TABLE [dbo].[OrderLines] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [ProductId] INT NOT NULL,
                        [Quantity] INT NOT NULL,
                        [Price] DECIMAL(18,2) NOT NULL,
                        [OrderId] INT NOT NULL,
                        CONSTRAINT [PK_OrderLines] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_OrderLines_Orders_OrderId] 
                            FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders]([Id]) 
                            ON DELETE CASCADE
                    );
                END
            ");

            // Indexes
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT * FROM sys.indexes 
                    WHERE object_id = OBJECT_ID(N'[dbo].[DeliveryAddresses]') 
                    AND name = N'IX_DeliveryAddresses_OrderId'
                )
                BEGIN
                    CREATE UNIQUE INDEX [IX_DeliveryAddresses_OrderId] 
                    ON [dbo].[DeliveryAddresses] ([OrderId]);
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT * FROM sys.indexes 
                    WHERE object_id = OBJECT_ID(N'[dbo].[OrderLines]') 
                    AND name = N'IX_OrderLines_OrderId'
                )
                BEGIN
                    CREATE INDEX [IX_OrderLines_OrderId] 
                    ON [dbo].[OrderLines] ([OrderId]);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[DeliveryAddresses]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[DeliveryAddresses];
                END
            ");

            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[OrderLines]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[OrderLines];
                END
            ");

            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[Orders]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Orders];
                END
            ");
        }
    }
}
