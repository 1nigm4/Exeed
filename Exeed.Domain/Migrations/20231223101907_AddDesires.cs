using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exeed.Migrations
{
    public partial class AddDesires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesireId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Desires",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desires", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DesireId",
                table: "Accounts",
                column: "DesireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Desires_DesireId",
                table: "Accounts",
                column: "DesireId",
                principalTable: "Desires",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Desires_DesireId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Desires");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_DesireId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DesireId",
                table: "Accounts");
        }
    }
}
