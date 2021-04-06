using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class PlayerStateDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerState_Games_GameStateId",
                table: "PlayerState");

            migrationBuilder.AlterColumn<int>(
                name: "GameStateId",
                table: "PlayerState",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerState_Games_GameStateId",
                table: "PlayerState",
                column: "GameStateId",
                principalTable: "Games",
                principalColumn: "GameStateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerState_Games_GameStateId",
                table: "PlayerState");

            migrationBuilder.AlterColumn<int>(
                name: "GameStateId",
                table: "PlayerState",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerState_Games_GameStateId",
                table: "PlayerState",
                column: "GameStateId",
                principalTable: "Games",
                principalColumn: "GameStateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
