using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPS_Final_Version.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GAME__PLAYER_ONE__5441852A",
                table: "GAME");

            migrationBuilder.DropForeignKey(
                name: "FK__GAME__PLAYER_TWO__5535A963",
                table: "GAME");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__GAMEID__5CD6CB2B",
                table: "ROUND");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__PLAYER_ON__5DCAEF64",
                table: "ROUND");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__PLAYER_TW__5EBF139D",
                table: "ROUND");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ROUND__C3DB5F0D1F7691B7",
                table: "ROUND");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PLAYER__B15BE12F8026C15E",
                table: "PLAYER");

            migrationBuilder.DropPrimaryKey(
                name: "PK__CHOICE__4193D92F9E9FC0A0",
                table: "CHOICE");

            migrationBuilder.DropColumn(
                name: "GAMER_WINNER",
                table: "GAME");

            migrationBuilder.AlterColumn<int>(
                name: "ROUNDNUMBER",
                table: "ROUND",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "WINNER",
                table: "ROUND",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PLAYER_TWO",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PLAYER_ONE",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GAMECODE",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATETIMEENDED",
                table: "GAME",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "GAMEID",
                table: "GAME",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "GAME_WINNER",
                table: "GAME",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__ROUND__C3DB5F0D48FFB294",
                table: "ROUND",
                columns: new[] { "ROUNDNUMBER", "GAMEID", "PLAYER_ONE_CHOICE", "PLAYER_TWO_CHOICE" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__PLAYER__B15BE12FD01F52A9",
                table: "PLAYER",
                column: "USERNAME");

            migrationBuilder.AddPrimaryKey(
                name: "PK__CHOICE__4193D92F491B2636",
                table: "CHOICE",
                column: "DESCRIPTION");

            migrationBuilder.AddForeignKey(
                name: "FK__GAME__PLAYER_ONE__503BEA1C",
                table: "GAME",
                column: "PLAYER_ONE",
                principalTable: "PLAYER",
                principalColumn: "USERNAME");

            migrationBuilder.AddForeignKey(
                name: "FK__GAME__PLAYER_TWO__51300E55",
                table: "GAME",
                column: "PLAYER_TWO",
                principalTable: "PLAYER",
                principalColumn: "USERNAME");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__GAMEID__59C55456",
                table: "ROUND",
                column: "GAMEID",
                principalTable: "GAME",
                principalColumn: "GAMEID");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__PLAYER_ON__5AB9788F",
                table: "ROUND",
                column: "PLAYER_ONE_CHOICE",
                principalTable: "CHOICE",
                principalColumn: "DESCRIPTION");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__PLAYER_TW__5BAD9CC8",
                table: "ROUND",
                column: "PLAYER_TWO_CHOICE",
                principalTable: "CHOICE",
                principalColumn: "DESCRIPTION");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GAME__PLAYER_ONE__503BEA1C",
                table: "GAME");

            migrationBuilder.DropForeignKey(
                name: "FK__GAME__PLAYER_TWO__51300E55",
                table: "GAME");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__GAMEID__59C55456",
                table: "ROUND");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__PLAYER_ON__5AB9788F",
                table: "ROUND");

            migrationBuilder.DropForeignKey(
                name: "FK__ROUND__PLAYER_TW__5BAD9CC8",
                table: "ROUND");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ROUND__C3DB5F0D48FFB294",
                table: "ROUND");

            migrationBuilder.DropPrimaryKey(
                name: "PK__PLAYER__B15BE12FD01F52A9",
                table: "PLAYER");

            migrationBuilder.DropPrimaryKey(
                name: "PK__CHOICE__4193D92F491B2636",
                table: "CHOICE");

            migrationBuilder.DropColumn(
                name: "WINNER",
                table: "ROUND");

            migrationBuilder.DropColumn(
                name: "GAME_WINNER",
                table: "GAME");

            migrationBuilder.AlterColumn<int>(
                name: "ROUNDNUMBER",
                table: "ROUND",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "PLAYER_TWO",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "PLAYER_ONE",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "GAMECODE",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DATETIMEENDED",
                table: "GAME",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GAMEID",
                table: "GAME",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "GAMER_WINNER",
                table: "GAME",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__ROUND__C3DB5F0D1F7691B7",
                table: "ROUND",
                columns: new[] { "ROUNDNUMBER", "GAMEID", "PLAYER_ONE_CHOICE", "PLAYER_TWO_CHOICE" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__PLAYER__B15BE12F8026C15E",
                table: "PLAYER",
                column: "USERNAME");

            migrationBuilder.AddPrimaryKey(
                name: "PK__CHOICE__4193D92F9E9FC0A0",
                table: "CHOICE",
                column: "DESCRIPTION");

            migrationBuilder.AddForeignKey(
                name: "FK__GAME__PLAYER_ONE__5441852A",
                table: "GAME",
                column: "PLAYER_ONE",
                principalTable: "PLAYER",
                principalColumn: "USERNAME");

            migrationBuilder.AddForeignKey(
                name: "FK__GAME__PLAYER_TWO__5535A963",
                table: "GAME",
                column: "PLAYER_TWO",
                principalTable: "PLAYER",
                principalColumn: "USERNAME");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__GAMEID__5CD6CB2B",
                table: "ROUND",
                column: "GAMEID",
                principalTable: "GAME",
                principalColumn: "GAMEID");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__PLAYER_ON__5DCAEF64",
                table: "ROUND",
                column: "PLAYER_ONE_CHOICE",
                principalTable: "CHOICE",
                principalColumn: "DESCRIPTION");

            migrationBuilder.AddForeignKey(
                name: "FK__ROUND__PLAYER_TW__5EBF139D",
                table: "ROUND",
                column: "PLAYER_TWO_CHOICE",
                principalTable: "CHOICE",
                principalColumn: "DESCRIPTION");
        }
    }
}
