using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrangeFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeUnit",
                table: "Harvests",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeUnit",
                table: "Harvests");
        }
    }
}
