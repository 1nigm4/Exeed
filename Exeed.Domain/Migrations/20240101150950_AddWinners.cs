using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exeed.Migrations
{
    public partial class AddWinners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Winners",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DesireId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WonAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Prize = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winners_Desires_DesireId",
                        column: x => x.DesireId,
                        principalTable: "Desires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Winners_DesireId",
                table: "Winners",
                column: "DesireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Winners");
        }
    }
}
