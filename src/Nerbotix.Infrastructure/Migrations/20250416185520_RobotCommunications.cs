using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RobotCommunications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Headers",
                table: "robot_communications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Headers",
                table: "robot_communications");
        }
    }
}
