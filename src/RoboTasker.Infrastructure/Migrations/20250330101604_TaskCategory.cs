using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TaskCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "robot_tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_robot_tasks_CategoryId",
                table: "robot_tasks",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks",
                column: "CategoryId",
                principalTable: "robot_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks");

            migrationBuilder.DropIndex(
                name: "IX_robot_tasks_CategoryId",
                table: "robot_tasks");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "robot_tasks");
        }
    }
}
