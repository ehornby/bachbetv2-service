using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClaimStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeClaims_Users_WitnessId",
                table: "ChallengeClaims");

            migrationBuilder.AlterColumn<int>(
                name: "WitnessId",
                table: "ChallengeClaims",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ChallengeClaims",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeClaims_Users_WitnessId",
                table: "ChallengeClaims",
                column: "WitnessId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeClaims_Users_WitnessId",
                table: "ChallengeClaims");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ChallengeClaims");

            migrationBuilder.AlterColumn<int>(
                name: "WitnessId",
                table: "ChallengeClaims",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeClaims_Users_WitnessId",
                table: "ChallengeClaims",
                column: "WitnessId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
