using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SuperTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tenants_Email",
                table: "tenants",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tenants_Email",
                table: "tenants");
        }
    }
}
