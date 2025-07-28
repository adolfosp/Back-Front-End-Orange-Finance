using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrangeFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTypeAndAddNewEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finances_Harvests_HarvestId",
                table: "Finances");

            migrationBuilder.AlterColumn<string>(
                name: "TypeTransaction",
                table: "Finances",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "Finances",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Finances_Harvests_HarvestId",
                table: "Finances",
                column: "HarvestId",
                principalTable: "Harvests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finances_Harvests_HarvestId",
                table: "Finances");

            migrationBuilder.AlterColumn<int>(
                name: "TypeTransaction",
                table: "Finances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Finances",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddForeignKey(
                name: "FK_Finances_Harvests_HarvestId",
                table: "Finances",
                column: "HarvestId",
                principalTable: "Harvests",
                principalColumn: "Id");
        }
    }
}
