using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferMessageToLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransferMessage",
                table: "Ledger",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferMessage",
                table: "Ledger");
        }
    }
}
