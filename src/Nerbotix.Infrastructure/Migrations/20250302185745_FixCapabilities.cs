using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCapabilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobotCapability_capabilities_CapabilityId",
                table: "RobotCapability");

            migrationBuilder.DropForeignKey(
                name: "FK_RobotCapability_robots_RobotId",
                table: "RobotCapability");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RobotCapability",
                table: "RobotCapability");

            migrationBuilder.DropIndex(
                name: "IX_RobotCapability_RobotId",
                table: "RobotCapability");

            migrationBuilder.RenameTable(
                name: "RobotCapability",
                newName: "robots_capabilities");

            migrationBuilder.RenameIndex(
                name: "IX_RobotCapability_CapabilityId",
                table: "robots_capabilities",
                newName: "IX_robots_capabilities_CapabilityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_robots_capabilities",
                table: "robots_capabilities",
                columns: new[] { "RobotId", "CapabilityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_robots_capabilities_capabilities_CapabilityId",
                table: "robots_capabilities",
                column: "CapabilityId",
                principalTable: "capabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_robots_capabilities_robots_RobotId",
                table: "robots_capabilities",
                column: "RobotId",
                principalTable: "robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_robots_capabilities_capabilities_CapabilityId",
                table: "robots_capabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_robots_capabilities_robots_RobotId",
                table: "robots_capabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_robots_capabilities",
                table: "robots_capabilities");

            migrationBuilder.RenameTable(
                name: "robots_capabilities",
                newName: "RobotCapability");

            migrationBuilder.RenameIndex(
                name: "IX_robots_capabilities_CapabilityId",
                table: "RobotCapability",
                newName: "IX_RobotCapability_CapabilityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RobotCapability",
                table: "RobotCapability",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RobotCapability_RobotId",
                table: "RobotCapability",
                column: "RobotId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobotCapability_capabilities_CapabilityId",
                table: "RobotCapability",
                column: "CapabilityId",
                principalTable: "capabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RobotCapability_robots_RobotId",
                table: "RobotCapability",
                column: "RobotId",
                principalTable: "robots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
