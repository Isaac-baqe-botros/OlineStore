using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elhoot_HomeDevices.Data.Migrations
{
    /// <inheritdoc />
    public partial class dyan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountMonth",
                table: "dayeenateys",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "selecteddataDyants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MadunatID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    MadunaateId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFree = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Paypalce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayeenateyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_selecteddataDyants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_selecteddataDyants_dayeenateys_DayeenateyId",
                        column: x => x.DayeenateyId,
                        principalTable: "dayeenateys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_selecteddataDyants_madunaates_MadunaateId",
                        column: x => x.MadunaateId,
                        principalTable: "madunaates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_selecteddataDyants_DayeenateyId",
                table: "selecteddataDyants",
                column: "DayeenateyId");

            migrationBuilder.CreateIndex(
                name: "IX_selecteddataDyants_MadunaateId",
                table: "selecteddataDyants",
                column: "MadunaateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "selecteddataDyants");

            migrationBuilder.DropColumn(
                name: "CountMonth",
                table: "dayeenateys");
        }
    }
}
