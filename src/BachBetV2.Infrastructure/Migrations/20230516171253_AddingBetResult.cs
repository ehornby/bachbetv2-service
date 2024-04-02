using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingBetResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Result",
                table: "Bets",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "Bets");
        }
    }
}
