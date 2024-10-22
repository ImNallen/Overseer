using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Overseer.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_Username_FirstName_LastName",
                schema: "public",
                table: "Users",
                columns: new[] { "Email", "Username", "FirstName", "LastName" })
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:TsVectorConfig", "english");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email_Username_FirstName_LastName",
                schema: "public",
                table: "Users");
        }
    }
}
