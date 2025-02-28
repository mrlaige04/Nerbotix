using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PropValueMigrateToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "robot_properties_values",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "robot_custom_properties",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "robot_properties_values",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "robot_custom_properties",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
