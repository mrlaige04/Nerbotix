using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SystemRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "permissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "permissions");
        }
    }
}
