using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mediporta.Data.Migrations
{
    /// <inheritdoc />
    public partial class TagPercentageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PercentageOfAll",
                table: "Tags",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageOfAll",
                table: "Tags");
        }
    }
}
