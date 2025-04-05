using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_robots_Name",
                table: "robots");

            migrationBuilder.DropIndex(
                name: "IX_robot_categories_Name",
                table: "robot_categories");

            migrationBuilder.CreateIndex(
                name: "IX_robots_TenantId_Name",
                table: "robots",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_categories_TenantId_Name",
                table: "robot_categories",
                columns: new[] { "TenantId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_robots_TenantId_Name",
                table: "robots");

            migrationBuilder.DropIndex(
                name: "IX_robot_categories_TenantId_Name",
                table: "robot_categories");

            migrationBuilder.CreateIndex(
                name: "IX_robots_Name",
                table: "robots",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_categories_Name",
                table: "robot_categories",
                column: "Name",
                unique: true);
        }
    }
}
