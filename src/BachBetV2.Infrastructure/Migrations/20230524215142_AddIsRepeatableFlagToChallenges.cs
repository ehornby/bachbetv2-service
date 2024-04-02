using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BachBetV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRepeatableFlagToChallenges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRepeatable",
                table: "Challenges",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRepeatable",
                table: "Challenges");
        }
    }
}
