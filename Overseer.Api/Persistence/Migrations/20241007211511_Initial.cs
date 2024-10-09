using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
            name: "Outbox_Message",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<string>(type: "text", nullable: false),
                Content = table.Column<string>(type: "jsonb", maxLength: 4000, nullable: false),
                OccurredOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Error = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Outbox_Message", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Permissions",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                Username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Password = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                EmailVerificationToken = table.Column<Guid>(type: "uuid", nullable: true),
                EmailVerificationTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                PasswordResetToken = table.Column<Guid>(type: "uuid", nullable: true),
                PasswordResetTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RefreshToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                RefreshTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Status = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RolePermissions",
            schema: "public",
            columns: table => new
            {
                RoleId = table.Column<int>(type: "integer", nullable: false),
                PermissionId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_RolePermissions_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalSchema: "public",
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "public",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoleUser",
            schema: "public",
            columns: table => new
            {
                RolesId = table.Column<int>(type: "integer", nullable: false),
                UsersId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                table.ForeignKey(
                    name: "FK_RoleUser_Roles_RolesId",
                    column: x => x.RolesId,
                    principalSchema: "public",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoleUser_Users_UsersId",
                    column: x => x.UsersId,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            schema: "public",
            table: "Permissions",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { 1, "users:read" },
                { 2, "users:write" },
                { 3, "users:delete" },
                { 4, "admin" }
            });

        migrationBuilder.InsertData(
            schema: "public",
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { 1, "Admin" },
                { 2, "User" }
            });

        migrationBuilder.InsertData(
            schema: "public",
            table: "RolePermissions",
            columns: new[] { "PermissionId", "RoleId" },
            values: new object[,]
            {
                { 1, 1 },
                { 2, 1 },
                { 3, 1 },
                { 4, 1 },
                { 1, 2 },
                { 2, 2 },
                { 3, 2 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_PermissionId",
            schema: "public",
            table: "RolePermissions",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleUser_UsersId",
            schema: "public",
            table: "RoleUser",
            column: "UsersId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            schema: "public",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_EmailVerificationToken",
            schema: "public",
            table: "Users",
            column: "EmailVerificationToken",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_PasswordResetToken",
            schema: "public",
            table: "Users",
            column: "PasswordResetToken",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_RefreshToken",
            schema: "public",
            table: "Users",
            column: "RefreshToken",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            schema: "public",
            table: "Users",
            column: "Username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Outbox_Message",
            schema: "public");

        migrationBuilder.DropTable(
            name: "RolePermissions",
            schema: "public");

        migrationBuilder.DropTable(
            name: "RoleUser",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Permissions",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Roles",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "public");
    }
}
