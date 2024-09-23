using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overseer.Api.Persistence.Migrations;

/// <inheritdoc />
public partial class AddPermission : Migration
{
    private static readonly string[] Columns = new[] { "PermissionId", "RoleId" };
    private static readonly string[] ColumnsA = new[] { "Id", "Name", "RoleId" };

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Permissions",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false),
                RoleId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Permissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "public",
                    principalTable: "Roles",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "RolePermissions",
            schema: "public",
            columns: table => new
            {
                RoleId = table.Column<int>(type: "integer", nullable: false),
                PermissionId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId }));

        migrationBuilder.InsertData(
            schema: "public",
            table: "Permissions",
            columns: ColumnsA,
            values: new object[,]
            {
                { 1, "users:read", null },
                { 2, "users:write", null }
            });

        migrationBuilder.InsertData(
            schema: "public",
            table: "RolePermissions",
            columns: Columns,
            values: new object[,]
            {
                { 1, 1 },
                { 2, 1 },
                { 1, 2 },
                { 2, 2 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Permissions_RoleId",
            schema: "public",
            table: "Permissions",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Permissions",
            schema: "public");

        migrationBuilder.DropTable(
            name: "RolePermissions",
            schema: "public");
    }
}
