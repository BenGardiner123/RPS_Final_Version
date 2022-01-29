using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPS_Final_Version.Migrations
{
    public partial class notSure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CHOICE",
                columns: table => new
                {
                    DESCRIPTION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CHOICE__4193D92F9E9FC0A0", x => x.DESCRIPTION);
                });

            migrationBuilder.CreateTable(
                name: "PLAYER",
                columns: table => new
                {
                    USERNAME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PLAYER__B15BE12F8026C15E", x => x.USERNAME);
                });

            migrationBuilder.CreateTable(
                name: "GAME",
                columns: table => new
                {
                    GAMEID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GAMECODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GAMER_WINNER = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ROUNDLIMIT = table.Column<int>(type: "int", nullable: false),
                    DATETIMESTARTED = table.Column<DateTime>(type: "datetime", nullable: false),
                    DATETIMEENDED = table.Column<DateTime>(type: "datetime", nullable: false),
                    PLAYER_ONE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PLAYER_TWO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAME", x => x.GAMEID);
                    table.ForeignKey(
                        name: "FK__GAME__PLAYER_ONE__5441852A",
                        column: x => x.PLAYER_ONE,
                        principalTable: "PLAYER",
                        principalColumn: "USERNAME");
                    table.ForeignKey(
                        name: "FK__GAME__PLAYER_TWO__5535A963",
                        column: x => x.PLAYER_TWO,
                        principalTable: "PLAYER",
                        principalColumn: "USERNAME");
                });

            migrationBuilder.CreateTable(
                name: "ROUND",
                columns: table => new
                {
                    ROUNDNUMBER = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GAMEID = table.Column<int>(type: "int", nullable: false),
                    PLAYER_ONE_CHOICE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PLAYER_TWO_CHOICE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ROUND__C3DB5F0D1F7691B7", x => new { x.ROUNDNUMBER, x.GAMEID, x.PLAYER_ONE_CHOICE, x.PLAYER_TWO_CHOICE });
                    table.ForeignKey(
                        name: "FK__ROUND__GAMEID__5CD6CB2B",
                        column: x => x.GAMEID,
                        principalTable: "GAME",
                        principalColumn: "GAMEID");
                    table.ForeignKey(
                        name: "FK__ROUND__PLAYER_ON__5DCAEF64",
                        column: x => x.PLAYER_ONE_CHOICE,
                        principalTable: "CHOICE",
                        principalColumn: "DESCRIPTION");
                    table.ForeignKey(
                        name: "FK__ROUND__PLAYER_TW__5EBF139D",
                        column: x => x.PLAYER_TWO_CHOICE,
                        principalTable: "CHOICE",
                        principalColumn: "DESCRIPTION");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GAME_PLAYER_ONE",
                table: "GAME",
                column: "PLAYER_ONE");

            migrationBuilder.CreateIndex(
                name: "IX_GAME_PLAYER_TWO",
                table: "GAME",
                column: "PLAYER_TWO");

            migrationBuilder.CreateIndex(
                name: "IX_ROUND_GAMEID",
                table: "ROUND",
                column: "GAMEID");

            migrationBuilder.CreateIndex(
                name: "IX_ROUND_PLAYER_ONE_CHOICE",
                table: "ROUND",
                column: "PLAYER_ONE_CHOICE");

            migrationBuilder.CreateIndex(
                name: "IX_ROUND_PLAYER_TWO_CHOICE",
                table: "ROUND",
                column: "PLAYER_TWO_CHOICE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROUND");

            migrationBuilder.DropTable(
                name: "GAME");

            migrationBuilder.DropTable(
                name: "CHOICE");

            migrationBuilder.DropTable(
                name: "PLAYER");
        }
    }
}
