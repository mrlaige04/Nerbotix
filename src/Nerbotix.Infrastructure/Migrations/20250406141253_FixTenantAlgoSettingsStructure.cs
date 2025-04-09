using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTenantAlgoSettingsStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimulatedAnnealingAlgorithmSettings_MinTemperature",
                table: "tenant_settings",
                newName: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_MinTempe~");

            migrationBuilder.RenameColumn(
                name: "SimulatedAnnealingAlgorithmSettings_IterationsPerTemp",
                table: "tenant_settings",
                newName: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_Iteratio~");

            migrationBuilder.RenameColumn(
                name: "SimulatedAnnealingAlgorithmSettings_InitialTemperature",
                table: "tenant_settings",
                newName: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_InitialT~");

            migrationBuilder.RenameColumn(
                name: "SimulatedAnnealingAlgorithmSettings_CoolingRate",
                table: "tenant_settings",
                newName: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_CoolingR~");

            migrationBuilder.RenameColumn(
                name: "PreferredAlgorithm",
                table: "tenant_settings",
                newName: "AlgorithmSettings_PreferredAlgorithm");

            migrationBuilder.RenameColumn(
                name: "LoadBalancingAlgorithmSettings_ComplexityFactor",
                table: "tenant_settings",
                newName: "AlgorithmSettings_LoadBalancingAlgorithmSettings_ComplexityFac~");

            migrationBuilder.RenameColumn(
                name: "GeneticAlgorithmSettings_PopulationSize",
                table: "tenant_settings",
                newName: "AlgorithmSettings_GeneticAlgorithmSettings_PopulationSize");

            migrationBuilder.RenameColumn(
                name: "GeneticAlgorithmSettings_MutationRate",
                table: "tenant_settings",
                newName: "AlgorithmSettings_GeneticAlgorithmSettings_MutationRate");

            migrationBuilder.RenameColumn(
                name: "GeneticAlgorithmSettings_Generations",
                table: "tenant_settings",
                newName: "AlgorithmSettings_GeneticAlgorithmSettings_Generations");

            migrationBuilder.RenameColumn(
                name: "AntColonyAlgorithmSettings_Iterations",
                table: "tenant_settings",
                newName: "AlgorithmSettings_AntColonyAlgorithmSettings_Iterations");

            migrationBuilder.RenameColumn(
                name: "AntColonyAlgorithmSettings_Evaporation",
                table: "tenant_settings",
                newName: "AlgorithmSettings_AntColonyAlgorithmSettings_Evaporation");

            migrationBuilder.RenameColumn(
                name: "AntColonyAlgorithmSettings_Beta",
                table: "tenant_settings",
                newName: "AlgorithmSettings_AntColonyAlgorithmSettings_Beta");

            migrationBuilder.RenameColumn(
                name: "AntColonyAlgorithmSettings_AntCount",
                table: "tenant_settings",
                newName: "AlgorithmSettings_AntColonyAlgorithmSettings_AntCount");

            migrationBuilder.RenameColumn(
                name: "AntColonyAlgorithmSettings_Alpha",
                table: "tenant_settings",
                newName: "AlgorithmSettings_AntColonyAlgorithmSettings_Alpha");

            migrationBuilder.AddColumn<DateTime>(
                name: "AlgorithmSettings_CreatedAt",
                table: "tenant_settings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "AlgorithmSettings_Id",
                table: "tenant_settings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AlgorithmSettings_TenantId",
                table: "tenant_settings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AlgorithmSettings_UpdatedAt",
                table: "tenant_settings",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlgorithmSettings_CreatedAt",
                table: "tenant_settings");

            migrationBuilder.DropColumn(
                name: "AlgorithmSettings_Id",
                table: "tenant_settings");

            migrationBuilder.DropColumn(
                name: "AlgorithmSettings_TenantId",
                table: "tenant_settings");

            migrationBuilder.DropColumn(
                name: "AlgorithmSettings_UpdatedAt",
                table: "tenant_settings");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_MinTempe~",
                table: "tenant_settings",
                newName: "SimulatedAnnealingAlgorithmSettings_MinTemperature");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_Iteratio~",
                table: "tenant_settings",
                newName: "SimulatedAnnealingAlgorithmSettings_IterationsPerTemp");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_InitialT~",
                table: "tenant_settings",
                newName: "SimulatedAnnealingAlgorithmSettings_InitialTemperature");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_SimulatedAnnealingAlgorithmSettings_CoolingR~",
                table: "tenant_settings",
                newName: "SimulatedAnnealingAlgorithmSettings_CoolingRate");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_PreferredAlgorithm",
                table: "tenant_settings",
                newName: "PreferredAlgorithm");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_LoadBalancingAlgorithmSettings_ComplexityFac~",
                table: "tenant_settings",
                newName: "LoadBalancingAlgorithmSettings_ComplexityFactor");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_GeneticAlgorithmSettings_PopulationSize",
                table: "tenant_settings",
                newName: "GeneticAlgorithmSettings_PopulationSize");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_GeneticAlgorithmSettings_MutationRate",
                table: "tenant_settings",
                newName: "GeneticAlgorithmSettings_MutationRate");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_GeneticAlgorithmSettings_Generations",
                table: "tenant_settings",
                newName: "GeneticAlgorithmSettings_Generations");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_AntColonyAlgorithmSettings_Iterations",
                table: "tenant_settings",
                newName: "AntColonyAlgorithmSettings_Iterations");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_AntColonyAlgorithmSettings_Evaporation",
                table: "tenant_settings",
                newName: "AntColonyAlgorithmSettings_Evaporation");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_AntColonyAlgorithmSettings_Beta",
                table: "tenant_settings",
                newName: "AntColonyAlgorithmSettings_Beta");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_AntColonyAlgorithmSettings_AntCount",
                table: "tenant_settings",
                newName: "AntColonyAlgorithmSettings_AntCount");

            migrationBuilder.RenameColumn(
                name: "AlgorithmSettings_AntColonyAlgorithmSettings_Alpha",
                table: "tenant_settings",
                newName: "AntColonyAlgorithmSettings_Alpha");
        }
    }
}
