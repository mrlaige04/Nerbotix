using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Logs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Scope = table.Column<int>(type: "integer", nullable: false),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_logs_robot_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "robot_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_logs_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_logs_RobotId",
                table: "logs",
                column: "RobotId");

            migrationBuilder.CreateIndex(
                name: "IX_logs_TaskId",
                table: "logs",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "logs");
        }
    }
}
