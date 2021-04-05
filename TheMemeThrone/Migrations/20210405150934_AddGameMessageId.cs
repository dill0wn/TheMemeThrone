using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class AddGameMessageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_Guild",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "PlayerState",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Guild",
                table: "Games",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "Games",
                newName: "GuildId");

            migrationBuilder.AddColumn<ulong>(
                name: "ChannelId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GuildId",
                table: "Games",
                column: "GuildId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_GuildId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PlayerState",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Games",
                newName: "Guild");

            migrationBuilder.RenameColumn(
                name: "GuildId",
                table: "Games",
                newName: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Guild",
                table: "Games",
                column: "Guild",
                unique: true);
        }
    }
}
