using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Db.Migrations
{
    /// <inheritdoc />
    public partial class Version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckerThatPreformedTakingX",
                table: "CheckerGameStates",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheckerThatPreformedTakingY",
                table: "CheckerGameStates",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckerThatPreformedTakingX",
                table: "CheckerGameStates");

            migrationBuilder.DropColumn(
                name: "CheckerThatPreformedTakingY",
                table: "CheckerGameStates");
        }
    }
}
