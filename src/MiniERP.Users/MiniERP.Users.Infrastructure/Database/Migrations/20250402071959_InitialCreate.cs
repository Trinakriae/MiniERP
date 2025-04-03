using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniERP.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type = 'U')
                BEGIN
                    CREATE TABLE [dbo].[Users] (
                        [Id] INT IDENTITY(1,1) NOT NULL,
                        [FirstName] NVARCHAR(MAX) NOT NULL,
                        [LastName] NVARCHAR(MAX) NOT NULL,
                        [Email] NVARCHAR(MAX) NOT NULL,
                        [PhoneNumber] NVARCHAR(MAX) NOT NULL,
                        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[Users];
                END
            ");
        }
    }
}
