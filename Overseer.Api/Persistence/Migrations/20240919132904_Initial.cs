using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overseer.Api.Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            schema: "public",
            table: "Users",
            column: "Email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "Users",
            schema: "public");
}
