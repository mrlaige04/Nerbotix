using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TenantSettingsForAlgos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenant_settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoadBalancingAlgorithmSettings_ComplexityFactor = table.Column<double>(type: "double precision", nullable: false),
                    GeneticAlgorithmSettings_PopulationSize = table.Column<int>(type: "integer", nullable: false),
                    GeneticAlgorithmSettings_Generations = table.Column<int>(type: "integer", nullable: false),
                    GeneticAlgorithmSettings_MutationRate = table.Column<double>(type: "double precision", nullable: false),
                    AntColonyAlgorithmSettings_AntCount = table.Column<int>(type: "integer", nullable: false),
                    AntColonyAlgorithmSettings_Iterations = table.Column<int>(type: "integer", nullable: false),
                    AntColonyAlgorithmSettings_Evaporation = table.Column<double>(type: "double precision", nullable: false),
                    AntColonyAlgorithmSettings_Alpha = table.Column<double>(type: "double precision", nullable: false),
                    AntColonyAlgorithmSettings_Beta = table.Column<double>(type: "double precision", nullable: false),
                    SimulatedAnnealingAlgorithmSettings_InitialTemperature = table.Column<double>(type: "double precision", nullable: false),
                    SimulatedAnnealingAlgorithmSettings_CoolingRate = table.Column<double>(type: "double precision", nullable: false),
                    SimulatedAnnealingAlgorithmSettings_IterationsPerTemp = table.Column<int>(type: "integer", nullable: false),
                    SimulatedAnnealingAlgorithmSettings_MinTemperature = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant_settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tenant_settings_tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tenant_settings_TenantId",
                table: "tenant_settings",
                column: "TenantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenant_settings");
        }
    }
}
