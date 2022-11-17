using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckerGameOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    TakingIsMandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    WhiteStarts = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckerGameOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckerGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StarterAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GameOverAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    GameWonBy = table.Column<string>(type: "TEXT", nullable: true),
                    Player1Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Player1Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Player2Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Player2Type = table.Column<int>(type: "INTEGER", nullable: false),
                    GameOptionsId = table.Column<int>(type: "INTEGER", nullable: true),
                    OptionsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckerGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckerGames_CheckerGameOptions_GameOptionsId",
                        column: x => x.GameOptionsId,
                        principalTable: "CheckerGameOptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CheckerGameStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NextMoveByBlack = table.Column<bool>(type: "INTEGER", nullable: false),
                    SerializedGameBoard = table.Column<string>(type: "TEXT", nullable: false),
                    CheckerGameId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckerGameStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckerGameStates_CheckerGames_CheckerGameId",
                        column: x => x.CheckerGameId,
                        principalTable: "CheckerGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckerGames_GameOptionsId",
                table: "CheckerGames",
                column: "GameOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckerGameStates_CheckerGameId",
                table: "CheckerGameStates",
                column: "CheckerGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckerGameStates");

            migrationBuilder.DropTable(
                name: "CheckerGames");

            migrationBuilder.DropTable(
                name: "CheckerGameOptions");
        }
    }
}
