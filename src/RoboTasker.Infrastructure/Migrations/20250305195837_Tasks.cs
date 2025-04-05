using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentTaskId",
                table: "robots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "robot_tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssignedRobotId = table.Column<Guid>(type: "uuid", nullable: true),
                    EstimatedDuration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Complexity = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_robot_tasks_robots_AssignedRobotId",
                        column: x => x.AssignedRobotId,
                        principalTable: "robots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "task_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<bool>(type: "boolean", nullable: true),
                    RobotTaskDateTimeData_Value = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileType = table.Column<string>(type: "text", nullable: true),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true),
                    RobotTaskJsonData_Value = table.Column<string>(type: "jsonb", nullable: true),
                    RobotTaskNumberData_Value = table.Column<double>(type: "double precision", nullable: true),
                    RobotTaskStringData_Value = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_data_robot_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "robot_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_properties_robot_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "robot_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_requirements",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    CapabilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_requirements", x => new { x.TaskId, x.CapabilityId });
                    table.ForeignKey(
                        name: "FK_task_requirements_capabilities_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "capabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_requirements_robot_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "robot_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_robot_tasks_AssignedRobotId",
                table: "robot_tasks",
                column: "AssignedRobotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_data_TaskId",
                table: "task_data",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_properties_TaskId",
                table: "task_properties",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_requirements_CapabilityId",
                table: "task_requirements",
                column: "CapabilityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "task_data");

            migrationBuilder.DropTable(
                name: "task_properties");

            migrationBuilder.DropTable(
                name: "task_requirements");

            migrationBuilder.DropTable(
                name: "robot_tasks");

            migrationBuilder.DropColumn(
                name: "CurrentTaskId",
                table: "robots");
        }
    }
}
