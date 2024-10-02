using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Overseer.Api.Persistence.Migrations;

/// <inheritdoc />
public partial class AddOrganisations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            schema: "public",
            table: "RolePermissions",
            keyColumns: new[] { "PermissionId", "RoleId" },
            keyValues: new object[] { 1, 3 });

        migrationBuilder.DeleteData(
            schema: "public",
            table: "Roles",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            schema: "public",
            table: "Users",
            type: "character varying(255)",
            maxLength: 255,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255);

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            schema: "public",
            table: "Users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            schema: "public",
            table: "Users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50);

        migrationBuilder.AddColumn<Guid>(
            name: "InviteToken",
            schema: "public",
            table: "Users",
            type: "uuid",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "OrganisationId",
            schema: "public",
            table: "Users",
            type: "uuid",
            nullable: false,
            defaultValue: Guid.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Status",
            schema: "public",
            table: "Users",
            type: "text",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Username",
            schema: "public",
            table: "Users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.CreateTable(
            name: "Organisations",
            schema: "public",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Organisations", x => x.Id));

        migrationBuilder.InsertData(
            schema: "public",
            table: "RolePermissions",
            columns: new[] { "PermissionId", "RoleId" },
            values: new object[,]
            {
                { 4, 1 },
                { 3, 2 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Users_EmailVerificationToken",
            schema: "public",
            table: "Users",
            column: "EmailVerificationToken",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_InviteToken",
            schema: "public",
            table: "Users",
            column: "InviteToken",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_OrganisationId",
            schema: "public",
            table: "Users",
            column: "OrganisationId");

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

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Organisations_OrganisationId",
            schema: "public",
            table: "Users",
            column: "OrganisationId",
            principalSchema: "public",
            principalTable: "Organisations",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Organisations_OrganisationId",
            schema: "public",
            table: "Users");

        migrationBuilder.DropTable(
            name: "Organisations",
            schema: "public");

        migrationBuilder.DropIndex(
            name: "IX_Users_EmailVerificationToken",
            schema: "public",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_InviteToken",
            schema: "public",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_OrganisationId",
            schema: "public",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_PasswordResetToken",
            schema: "public",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_RefreshToken",
            schema: "public",
            table: "Users");

        migrationBuilder.DropIndex(
            name: "IX_Users_Username",
            schema: "public",
            table: "Users");

        migrationBuilder.DeleteData(
            schema: "public",
            table: "RolePermissions",
            keyColumns: new[] { "PermissionId", "RoleId" },
            keyValues: new object[] { 4, 1 });

        migrationBuilder.DeleteData(
            schema: "public",
            table: "RolePermissions",
            keyColumns: new[] { "PermissionId", "RoleId" },
            keyValues: new object[] { 3, 2 });

        migrationBuilder.DropColumn(
            name: "InviteToken",
            schema: "public",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "OrganisationId",
            schema: "public",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Status",
            schema: "public",
            table: "Users");

        migrationBuilder.DropColumn(
            name: "Username",
            schema: "public",
            table: "Users");

        migrationBuilder.AlterColumn<string>(
            name: "Password",
            schema: "public",
            table: "Users",
            type: "character varying(255)",
            maxLength: 255,
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "character varying(255)",
            oldMaxLength: 255,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            schema: "public",
            table: "Users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            schema: "public",
            table: "Users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "character varying(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.InsertData(
            schema: "public",
            table: "Roles",
            columns: new[] { "Id", "Name" },
            values: new object[] { 3, "ReadOnly" });

        migrationBuilder.InsertData(
            schema: "public",
            table: "RolePermissions",
            columns: new[] { "PermissionId", "RoleId" },
            values: new object[] { 1, 3 });
    }
}
