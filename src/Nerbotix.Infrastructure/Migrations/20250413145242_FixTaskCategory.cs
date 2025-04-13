using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTaskCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "robot_tasks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks",
                column: "CategoryId",
                principalTable: "robot_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "robot_tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_robot_tasks_robot_categories_CategoryId",
                table: "robot_tasks",
                column: "CategoryId",
                principalTable: "robot_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
