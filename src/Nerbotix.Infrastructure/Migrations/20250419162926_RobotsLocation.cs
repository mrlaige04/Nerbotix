using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RobotsLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobotLocation_robots_RobotId",
                table: "RobotLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RobotLocation",
                table: "RobotLocation");

            migrationBuilder.RenameTable(
                name: "RobotLocation",
                newName: "robot_locations");

            migrationBuilder.RenameIndex(
                name: "IX_RobotLocation_RobotId",
                table: "robot_locations",
                newName: "IX_robot_locations_RobotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_robot_locations",
                table: "robot_locations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_robot_locations_robots_RobotId",
                table: "robot_locations",
                column: "RobotId",
                principalTable: "robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_locations_robots_RobotId",
                table: "robot_locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_robot_locations",
                table: "robot_locations");

            migrationBuilder.RenameTable(
                name: "robot_locations",
                newName: "RobotLocation");

            migrationBuilder.RenameIndex(
                name: "IX_robot_locations_RobotId",
                table: "RobotLocation",
                newName: "IX_RobotLocation_RobotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RobotLocation",
                table: "RobotLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RobotLocation_robots_RobotId",
                table: "RobotLocation",
                column: "RobotId",
                principalTable: "robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
