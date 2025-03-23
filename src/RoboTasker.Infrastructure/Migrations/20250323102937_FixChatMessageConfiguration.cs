using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboTasker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChatMessageConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chat_messages_users_UserId",
                table: "chat_messages");

            migrationBuilder.DropIndex(
                name: "IX_chat_messages_UserId",
                table: "chat_messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_UserId",
                table: "chat_messages",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_chat_messages_users_UserId",
                table: "chat_messages",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
