using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrangeFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldOnFinance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Finances",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Finances");
        }
    }
}
