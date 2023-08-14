using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elhoot_HomeDevices.Data.Migrations
{
    /// <inheritdoc />
    public partial class dyan1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_selecteddataDyants_dayeenateys_DayeenateyId",
                table: "selecteddataDyants");

            migrationBuilder.DropForeignKey(
                name: "FK_selecteddataDyants_madunaates_MadunaateId",
                table: "selecteddataDyants");

            migrationBuilder.DropIndex(
                name: "IX_selecteddataDyants_MadunaateId",
                table: "selecteddataDyants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dayeenateys",
                table: "dayeenateys");

            migrationBuilder.DropColumn(
                name: "MadunaateId",
                table: "selecteddataDyants");

            migrationBuilder.RenameTable(
                name: "dayeenateys",
                newName: "Dayeenateys");

            migrationBuilder.RenameColumn(
                name: "MadunatID",
                table: "selecteddataDyants",
                newName: "DeenID");

            migrationBuilder.AlterColumn<int>(
                name: "DayeenateyId",
                table: "selecteddataDyants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dayeenateys",
                table: "Dayeenateys",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_selecteddataDyants_Dayeenateys_DayeenateyId",
                table: "selecteddataDyants",
                column: "DayeenateyId",
                principalTable: "Dayeenateys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_selecteddataDyants_Dayeenateys_DayeenateyId",
                table: "selecteddataDyants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dayeenateys",
                table: "Dayeenateys");

            migrationBuilder.RenameTable(
                name: "Dayeenateys",
                newName: "dayeenateys");

            migrationBuilder.RenameColumn(
                name: "DeenID",
                table: "selecteddataDyants",
                newName: "MadunatID");

            migrationBuilder.AlterColumn<int>(
                name: "DayeenateyId",
                table: "selecteddataDyants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MadunaateId",
                table: "selecteddataDyants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dayeenateys",
                table: "dayeenateys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_selecteddataDyants_MadunaateId",
                table: "selecteddataDyants",
                column: "MadunaateId");

            migrationBuilder.AddForeignKey(
                name: "FK_selecteddataDyants_dayeenateys_DayeenateyId",
                table: "selecteddataDyants",
                column: "DayeenateyId",
                principalTable: "dayeenateys",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_selecteddataDyants_madunaates_MadunaateId",
                table: "selecteddataDyants",
                column: "MadunaateId",
                principalTable: "madunaates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
