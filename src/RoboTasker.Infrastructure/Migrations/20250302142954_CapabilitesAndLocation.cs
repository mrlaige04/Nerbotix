using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CapabilitesAndLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivity",
                table: "robots",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "robots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "robots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "capability_groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_capability_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobotLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobotLocation_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "capabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_capabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_capabilities_capability_groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "capability_groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RobotCapability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RobotId = table.Column<Guid>(type: "uuid", nullable: false),
                    CapabilityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobotCapability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobotCapability_capabilities_CapabilityId",
                        column: x => x.CapabilityId,
                        principalTable: "capabilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobotCapability_robots_RobotId",
                        column: x => x.RobotId,
                        principalTable: "robots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_capabilities_GroupId",
                table: "capabilities",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RobotCapability_CapabilityId",
                table: "RobotCapability",
                column: "CapabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_RobotCapability_RobotId",
                table: "RobotCapability",
                column: "RobotId");

            migrationBuilder.CreateIndex(
                name: "IX_RobotLocation_RobotId",
                table: "RobotLocation",
                column: "RobotId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RobotCapability");

            migrationBuilder.DropTable(
                name: "RobotLocation");

            migrationBuilder.DropTable(
                name: "capabilities");

            migrationBuilder.DropTable(
                name: "capability_groups");

            migrationBuilder.DropColumn(
                name: "LastActivity",
                table: "robots");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "robots");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "robots");
        }
    }
}
