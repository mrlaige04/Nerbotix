using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "RobotTaskDateTimeData_Value",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "RobotTaskJsonData_Value",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "RobotTaskNumberData_Value",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "RobotTaskStringData_Value",
                table: "task_data");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "task_data");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "task_data",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "task_archives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_archives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_archives_robot_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "robot_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_archives_TaskId",
                table: "task_archives",
                column: "TaskId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_archives");

            migrationBuilder.AlterColumn<bool>(
                name: "Value",
                table: "task_data",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "task_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "task_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "task_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RobotTaskDateTimeData_Value",
                table: "task_data",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobotTaskJsonData_Value",
                table: "task_data",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RobotTaskNumberData_Value",
                table: "task_data",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobotTaskStringData_Value",
                table: "task_data",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "task_data",
                type: "text",
                nullable: true);
        }
    }
}
