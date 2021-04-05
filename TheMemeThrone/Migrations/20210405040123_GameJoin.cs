using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class GameJoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerState",
                columns: table => new
                {
                    PlayerStateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerState", x => x.PlayerStateId);
                    table.ForeignKey(
                        name: "FK_PlayerState_Games_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "Games",
                        principalColumn: "GameStateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerState_GameStateId",
                table: "PlayerState",
                column: "GameStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerState");
        }
    }
}
