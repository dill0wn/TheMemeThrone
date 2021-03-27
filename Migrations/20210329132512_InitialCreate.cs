using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaptionCards",
                columns: table => new
                {
                    CaptionCardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptionCards", x => x.CaptionCardId);
                });

            migrationBuilder.CreateTable(
                name: "MemeCards",
                columns: table => new
                {
                    MemeCardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Label = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeCards", x => x.MemeCardId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaptionCards");

            migrationBuilder.DropTable(
                name: "MemeCards");
        }
    }
}
