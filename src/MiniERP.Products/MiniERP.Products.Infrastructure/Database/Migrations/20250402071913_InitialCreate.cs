using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP.Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[Categories](
                        [Id] [int] IDENTITY(1,1) NOT NULL,
                        [Name] [nvarchar](max) NOT NULL,
                        CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
                    )
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[Products](
                        [Id] [int] IDENTITY(1,1) NOT NULL,
                        [Name] [nvarchar](max) NOT NULL,
                        [Description] [nvarchar](max) NOT NULL,
                        [UnitPrice] [decimal](18,2) NOT NULL,
                        [CategoryId] [int] NOT NULL,
                        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
                        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
                    )
                END
            ");

            // Check if the index on CategoryId exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = N'IX_Products_CategoryId')
                BEGIN
                    CREATE INDEX [IX_Products_CategoryId] ON [dbo].[Products] ([CategoryId])
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the Products table if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[Products]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Products]
                END
            ");

            // Drop the Categories table if it exists
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[Categories]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Categories]
                END
            ");
        }
    }
}
