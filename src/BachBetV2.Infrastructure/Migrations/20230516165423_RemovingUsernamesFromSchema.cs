using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovingUsernamesFromSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "BetCreator",
                table: "Bets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Ledger",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BetCreator",
                table: "Bets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
