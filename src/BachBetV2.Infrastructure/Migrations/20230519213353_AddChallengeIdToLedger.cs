using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChallengeIdToLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BetId",
                table: "Ledger",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ChallengeId",
                table: "Ledger",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Ledger");

            migrationBuilder.AlterColumn<int>(
                name: "BetId",
                table: "Ledger",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
