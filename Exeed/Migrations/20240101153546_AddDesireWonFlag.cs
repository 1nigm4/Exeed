using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exeed.Migrations
{
    public partial class AddDesireWonFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWon",
                table: "Desires",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWon",
                table: "Desires");
        }
    }
}
