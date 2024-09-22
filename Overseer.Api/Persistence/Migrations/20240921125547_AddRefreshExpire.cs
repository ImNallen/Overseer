using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overseer.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshExpire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpires",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenExpires",
                schema: "public",
                table: "Users");
        }
    }
}
