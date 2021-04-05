using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class CreateGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guild = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Channel = table.Column<ulong>(type: "INTEGER", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameStateId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_Guild",
                table: "Games",
                column: "Guild",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
