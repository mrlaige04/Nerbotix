using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectIndexPermGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_permission_groups_Name",
                table: "permission_groups");

            migrationBuilder.CreateIndex(
                name: "IX_permission_groups_TenantId_Name",
                table: "permission_groups",
                columns: new[] { "TenantId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_permission_groups_TenantId_Name",
                table: "permission_groups");

            migrationBuilder.CreateIndex(
                name: "IX_permission_groups_Name",
                table: "permission_groups",
                column: "Name",
                unique: true);
        }
    }
}
