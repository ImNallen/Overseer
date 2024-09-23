using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overseer.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissionAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "public",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_RoleId",
                schema: "public",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "public",
                table: "Permissions");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "public",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                schema: "public",
                table: "RolePermissions",
                column: "PermissionId",
                principalSchema: "public",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                schema: "public",
                table: "RolePermissions",
                column: "RoleId",
                principalSchema: "public",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Permissions_PermissionId",
                schema: "public",
                table: "RolePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                schema: "public",
                table: "RolePermissions");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_PermissionId",
                schema: "public",
                table: "RolePermissions");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                schema: "public",
                table: "Permissions",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "public",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Roles_RoleId",
                schema: "public",
                table: "Permissions",
                column: "RoleId",
                principalSchema: "public",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
