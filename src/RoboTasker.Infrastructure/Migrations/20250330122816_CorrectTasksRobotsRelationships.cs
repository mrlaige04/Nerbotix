using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTasksRobotsRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_tasks_robots_AssignedRobotId",
                table: "robot_tasks");

            migrationBuilder.DropIndex(
                name: "IX_robot_tasks_AssignedRobotId",
                table: "robot_tasks");

            migrationBuilder.DropColumn(
                name: "CurrentTaskId",
                table: "robots");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "robot_tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "robot_tasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_tasks_AssignedRobotId",
                table: "robot_tasks",
                column: "AssignedRobotId");

            migrationBuilder.AddForeignKey(
                name: "FK_robot_tasks_robots_AssignedRobotId",
                table: "robot_tasks",
                column: "AssignedRobotId",
                principalTable: "robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_tasks_robots_AssignedRobotId",
                table: "robot_tasks");

            migrationBuilder.DropIndex(
                name: "IX_robot_tasks_AssignedRobotId",
                table: "robot_tasks");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "robot_tasks");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "robot_tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentTaskId",
                table: "robots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_tasks_AssignedRobotId",
                table: "robot_tasks",
                column: "AssignedRobotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_robot_tasks_robots_AssignedRobotId",
                table: "robot_tasks",
                column: "AssignedRobotId",
                principalTable: "robots",
                principalColumn: "Id");
        }
    }
}
