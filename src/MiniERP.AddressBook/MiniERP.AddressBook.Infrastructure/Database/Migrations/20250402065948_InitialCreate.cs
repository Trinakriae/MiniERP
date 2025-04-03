using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP.AddressBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Addresses]') AND type = 'U')
                BEGIN
                    CREATE TABLE [dbo].[Addresses] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [Street] NVARCHAR(MAX) NOT NULL,
                        [City] NVARCHAR(MAX) NOT NULL,
                        [State] NVARCHAR(MAX) NOT NULL,
                        [PostalCode] NVARCHAR(MAX) NOT NULL,
                        [Country] NVARCHAR(MAX) NOT NULL,
                        [IsPrimary] BIT NOT NULL,
                        [UserId] INT NOT NULL,
                        CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[Addresses]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Addresses];
                END
            ");
        }
    }
}
