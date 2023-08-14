using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elhoot_HomeDevices.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedDateElsaahp_selecteddataDyants_SelecteddataDyantId",
                table: "SelectedDateElsaahp");

            migrationBuilder.DropIndex(
                name: "IX_SelectedDateElsaahp_SelecteddataDyantId",
                table: "SelectedDateElsaahp");

            migrationBuilder.DropColumn(
                name: "SelecteddataDyantId",
                table: "SelectedDateElsaahp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelecteddataDyantId",
                table: "SelectedDateElsaahp",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelectedDateElsaahp_SelecteddataDyantId",
                table: "SelectedDateElsaahp",
                column: "SelecteddataDyantId");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedDateElsaahp_selecteddataDyants_SelecteddataDyantId",
                table: "SelectedDateElsaahp",
                column: "SelecteddataDyantId",
                principalTable: "selecteddataDyants",
                principalColumn: "Id");
        }
    }
}
