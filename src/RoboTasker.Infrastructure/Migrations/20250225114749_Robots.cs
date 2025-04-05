using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Robots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "robot_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "robot_category_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_category_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_robot_category_properties_robot_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "robot_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "robots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_robots_robot_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "robot_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "robot_communications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApiEndpoint = table.Column<string>(type: "text", nullable: true),
                    HttpMethod = table.Column<string>(type: "text", nullable: true),
                    MqttBrokerAddress = table.Column<string>(type: "text", nullable: true),
                    MqttBrokerUsername = table.Column<string>(type: "text", nullable: true),
                    MqttBrokerPassword = table.Column<string>(type: "text", nullable: true),
                    MqttTopic = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_communications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_robot_communications_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "robot_custom_properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "json", nullable: false),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_custom_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_robot_custom_properties_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "robot_properties_values",
                columns: table => new
                {
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "json", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_robot_properties_values", x => new { x.RobotId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_robot_properties_values_robot_category_properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "robot_category_properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_robot_properties_values_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_robot_categories_Name",
                table: "robot_categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_category_properties_CategoryId",
                table: "robot_category_properties",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_robot_communications_RobotId",
                table: "robot_communications",
                column: "RobotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_custom_properties_RobotId_Name",
                table: "robot_custom_properties",
                columns: new[] { "RobotId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_robot_properties_values_PropertyId",
                table: "robot_properties_values",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_robots_CategoryId",
                table: "robots",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_robots_Name",
                table: "robots",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "robot_communications");

            migrationBuilder.DropTable(
                name: "robot_custom_properties");

            migrationBuilder.DropTable(
                name: "robot_properties_values");

            migrationBuilder.DropTable(
                name: "robot_category_properties");

            migrationBuilder.DropTable(
                name: "robots");

            migrationBuilder.DropTable(
                name: "robot_categories");
        }
    }
}
