using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back.Migrations
{
    /// <inheritdoc />
    public partial class renamedBudgetEntitytoBudgets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_Budget_BudgetId",
                table: "games");

            migrationBuilder.DropIndex(
                name: "IX_games_BudgetId",
                table: "games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budget",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "games");

            migrationBuilder.RenameTable(
                name: "Budget",
                newName: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Budgets",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_GameId",
                table: "Budgets",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_games_GameId",
                table: "Budgets",
                column: "GameId",
                principalTable: "games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_games_GameId",
                table: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_GameId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "Budget");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "games",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budget",
                table: "Budget",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_games_BudgetId",
                table: "games",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_games_Budget_BudgetId",
                table: "games",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id");
        }
    }
}
