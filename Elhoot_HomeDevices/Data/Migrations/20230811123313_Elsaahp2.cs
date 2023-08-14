using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elhoot_HomeDevices.Data.Migrations
{
    /// <inheritdoc />
    public partial class Elsaahp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelectedDateElsaahp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MadunatID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    PayPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MadunaateId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateFree = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Paypalce = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElsaahpId = table.Column<int>(type: "int", nullable: false),
                    SelecteddataDyantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedDateElsaahp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedDateElsaahp_Elsaahps_ElsaahpId",
                        column: x => x.ElsaahpId,
                        principalTable: "Elsaahps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedDateElsaahp_madunaates_MadunaateId",
                        column: x => x.MadunaateId,
                        principalTable: "madunaates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelectedDateElsaahp_selecteddataDyants_SelecteddataDyantId",
                        column: x => x.SelecteddataDyantId,
                        principalTable: "selecteddataDyants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectedDateElsaahp_ElsaahpId",
                table: "SelectedDateElsaahp",
                column: "ElsaahpId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedDateElsaahp_MadunaateId",
                table: "SelectedDateElsaahp",
                column: "MadunaateId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedDateElsaahp_SelecteddataDyantId",
                table: "SelectedDateElsaahp",
                column: "SelecteddataDyantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectedDateElsaahp");
        }
    }
}
