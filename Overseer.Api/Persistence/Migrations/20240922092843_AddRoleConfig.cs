using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overseer.Api.Persistence.Migrations;

    /// <inheritdoc />
public partial class AddRoleConfig : Migration
{
    private static readonly string[] Columns = new[] { "Id", "Name" };

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_RoleUser_Role_RolesId",
            schema: "public",
            table: "RoleUser");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Role",
            schema: "public",
            table: "Role");

        migrationBuilder.RenameTable(
            name: "Role",
            schema: "public",
            newName: "Roles",
            newSchema: "public");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Roles",
            schema: "public",
            table: "Roles",
            column: "Id");

        migrationBuilder.InsertData(
            schema: "public",
            table: "Roles",
            columns: Columns,
            values: new object[,]
            {
                { 1, "Admin" },
                { 2, "User" }
            });

        migrationBuilder.AddForeignKey(
            name: "FK_RoleUser_Roles_RolesId",
            schema: "public",
            table: "RoleUser",
            column: "RolesId",
            principalSchema: "public",
            principalTable: "Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_RoleUser_Roles_RolesId",
            schema: "public",
            table: "RoleUser");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Roles",
            schema: "public",
            table: "Roles");

        migrationBuilder.DeleteData(
            schema: "public",
            table: "Roles",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            schema: "public",
            table: "Roles",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.RenameTable(
            name: "Roles",
            schema: "public",
            newName: "Role",
            newSchema: "public");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Role",
            schema: "public",
            table: "Role",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_RoleUser_Role_RolesId",
            schema: "public",
            table: "RoleUser",
            column: "RolesId",
            principalSchema: "public",
            principalTable: "Role",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
