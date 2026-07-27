using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRecipeBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Database_Adjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeDishType_Recipes_RecipeId",
                table: "RecipeDishType");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Recipes_RecipeId",
                table: "RecipeIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeInstruction_Recipes_RecipeId",
                table: "RecipeInstruction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeInstruction",
                table: "RecipeInstruction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeDishType",
                table: "RecipeDishType");

            migrationBuilder.RenameTable(
                name: "RecipeInstruction",
                newName: "RecipeInstructions");

            migrationBuilder.RenameTable(
                name: "RecipeIngredient",
                newName: "RecipeIngredients");

            migrationBuilder.RenameTable(
                name: "RecipeDishType",
                newName: "RecipeDishTypes");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeInstruction_RecipeId",
                table: "RecipeInstructions",
                newName: "IX_RecipeInstructions_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredient_RecipeId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeDishType_RecipeId",
                table: "RecipeDishTypes",
                newName: "IX_RecipeDishTypes_RecipeId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecipeInstructions",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "RecipeIngredients",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeInstructions",
                table: "RecipeInstructions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeDishTypes",
                table: "RecipeDishTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeDishTypes_Recipes_RecipeId",
                table: "RecipeDishTypes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeInstructions_Recipes_RecipeId",
                table: "RecipeInstructions",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeDishTypes_Recipes_RecipeId",
                table: "RecipeDishTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeInstructions_Recipes_RecipeId",
                table: "RecipeInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeInstructions",
                table: "RecipeInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeIngredients",
                table: "RecipeIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipeDishTypes",
                table: "RecipeDishTypes");

            migrationBuilder.RenameTable(
                name: "RecipeInstructions",
                newName: "RecipeInstruction");

            migrationBuilder.RenameTable(
                name: "RecipeIngredients",
                newName: "RecipeIngredient");

            migrationBuilder.RenameTable(
                name: "RecipeDishTypes",
                newName: "RecipeDishType");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeInstructions_RecipeId",
                table: "RecipeInstruction",
                newName: "IX_RecipeInstruction_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_RecipeId",
                table: "RecipeIngredient",
                newName: "IX_RecipeIngredient_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeDishTypes_RecipeId",
                table: "RecipeDishType",
                newName: "IX_RecipeDishType_RecipeId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "RecipeInstruction",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Item",
                table: "RecipeIngredient",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeInstruction",
                table: "RecipeInstruction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeIngredient",
                table: "RecipeIngredient",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipeDishType",
                table: "RecipeDishType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeDishType_Recipes_RecipeId",
                table: "RecipeDishType",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Recipes_RecipeId",
                table: "RecipeIngredient",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeInstruction_Recipes_RecipeId",
                table: "RecipeInstruction",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
