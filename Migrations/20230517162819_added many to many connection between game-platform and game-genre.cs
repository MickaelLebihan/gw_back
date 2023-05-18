using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back.Migrations
{
    /// <inheritdoc />
    public partial class addedmanytomanyconnectionbetweengameplatformandgamegenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_DevPriorities_DevPriorityId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_DevStatuses_DevStatusId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameEngines_GameEngineId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Games_GameId",
                table: "Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Platforms_Games_GameId",
                table: "Platforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms");

            migrationBuilder.DropIndex(
                name: "IX_Platforms_GameId",
                table: "Platforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_GameId",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEngines",
                table: "GameEngines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevStatuses",
                table: "DevStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevPriorities",
                table: "DevPriorities");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Genres");

            migrationBuilder.RenameTable(
                name: "Platforms",
                newName: "platforms");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "genres");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "games");

            migrationBuilder.RenameTable(
                name: "GameEngines",
                newName: "gameengines");

            migrationBuilder.RenameTable(
                name: "DevStatuses",
                newName: "devstatuses");

            migrationBuilder.RenameTable(
                name: "DevPriorities",
                newName: "devpriorities");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GameEngineId",
                table: "games",
                newName: "IX_games_GameEngineId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_DevStatusId",
                table: "games",
                newName: "IX_games_DevStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_DevPriorityId",
                table: "games",
                newName: "IX_games_DevPriorityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_platforms",
                table: "platforms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_genres",
                table: "genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_games",
                table: "games",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gameengines",
                table: "gameengines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_devstatuses",
                table: "devstatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_devpriorities",
                table: "devpriorities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.GamesId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_GameGenre_games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenre_genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    PlatformsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.GamesId, x.PlatformsId });
                    table.ForeignKey(
                        name: "FK_GamePlatform_games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_platforms_PlatformsId",
                        column: x => x.PlatformsId,
                        principalTable: "platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenresId",
                table: "GameGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformsId",
                table: "GamePlatform",
                column: "PlatformsId");

            migrationBuilder.AddForeignKey(
                name: "FK_games_devpriorities_DevPriorityId",
                table: "games",
                column: "DevPriorityId",
                principalTable: "devpriorities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_devstatuses_DevStatusId",
                table: "games",
                column: "DevStatusId",
                principalTable: "devstatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_gameengines_GameEngineId",
                table: "games",
                column: "GameEngineId",
                principalTable: "gameengines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_devpriorities_DevPriorityId",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_games_devstatuses_DevStatusId",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_games_gameengines_GameEngineId",
                table: "games");

            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropPrimaryKey(
                name: "PK_platforms",
                table: "platforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_genres",
                table: "genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_games",
                table: "games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gameengines",
                table: "gameengines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_devstatuses",
                table: "devstatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_devpriorities",
                table: "devpriorities");

            migrationBuilder.RenameTable(
                name: "platforms",
                newName: "Platforms");

            migrationBuilder.RenameTable(
                name: "genres",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "games",
                newName: "Games");

            migrationBuilder.RenameTable(
                name: "gameengines",
                newName: "GameEngines");

            migrationBuilder.RenameTable(
                name: "devstatuses",
                newName: "DevStatuses");

            migrationBuilder.RenameTable(
                name: "devpriorities",
                newName: "DevPriorities");

            migrationBuilder.RenameIndex(
                name: "IX_games_GameEngineId",
                table: "Games",
                newName: "IX_Games_GameEngineId");

            migrationBuilder.RenameIndex(
                name: "IX_games_DevStatusId",
                table: "Games",
                newName: "IX_Games_DevStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_games_DevPriorityId",
                table: "Games",
                newName: "IX_Games_DevPriorityId");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Platforms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Genres",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platforms",
                table: "Platforms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genres",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEngines",
                table: "GameEngines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevStatuses",
                table: "DevStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevPriorities",
                table: "DevPriorities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_GameId",
                table: "Platforms",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_GameId",
                table: "Genres",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DevPriorities_DevPriorityId",
                table: "Games",
                column: "DevPriorityId",
                principalTable: "DevPriorities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_DevStatuses_DevStatusId",
                table: "Games",
                column: "DevStatusId",
                principalTable: "DevStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameEngines_GameEngineId",
                table: "Games",
                column: "GameEngineId",
                principalTable: "GameEngines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Games_GameId",
                table: "Genres",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Platforms_Games_GameId",
                table: "Platforms",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
