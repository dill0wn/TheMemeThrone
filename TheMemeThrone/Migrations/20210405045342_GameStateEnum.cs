using Microsoft.EntityFrameworkCore.Migrations;

namespace TheMemeThrone.Migrations
{
    public partial class GameStateEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "Games",
                type: "nvarchar(24)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Games",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "nvarchar(24)");
        }
    }
}
