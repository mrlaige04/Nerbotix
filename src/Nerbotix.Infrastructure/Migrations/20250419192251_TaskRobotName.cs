using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskRobotName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedRobotName",
                table: "robot_tasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedRobotName",
                table: "robot_tasks");
        }
    }
}
