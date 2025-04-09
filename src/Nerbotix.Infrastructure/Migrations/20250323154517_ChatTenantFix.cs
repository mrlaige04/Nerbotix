using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nerbotix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChatTenantFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_chats_TenantId",
                table: "chats",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_chats_tenants_TenantId",
                table: "chats",
                column: "TenantId",
                principalTable: "tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chats_tenants_TenantId",
                table: "chats");

            migrationBuilder.DropIndex(
                name: "IX_chats_TenantId",
                table: "chats");
        }
    }
}
