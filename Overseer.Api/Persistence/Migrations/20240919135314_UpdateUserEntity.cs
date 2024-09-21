using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overseer.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmailVerificationToken",
                schema: "public",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiresAt",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PasswordResetToken",
                schema: "public",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpires",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpiresAt",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpires",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "public",
                table: "Users");
        }
    }
}
