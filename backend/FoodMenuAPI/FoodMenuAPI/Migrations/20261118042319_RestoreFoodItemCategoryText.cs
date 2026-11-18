using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodMenuAPI.Migrations
{
    public partial class RestoreFoodItemCategoryText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add the new Category column temporarily as nullable.
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            // 2. Copy category names before deleting the relationship.
            migrationBuilder.Sql(@"
                UPDATE FoodItems
                SET Category = FoodCategories.Name
                FROM FoodItems
                INNER JOIN FoodCategories
                    ON FoodItems.CategoryId = FoodCategories.Id;
            ");

            // 3. Make sure every food item received a category.
            migrationBuilder.Sql(@"
                IF EXISTS (
                    SELECT 1
                    FROM FoodItems
                    WHERE Category IS NULL
                )
                BEGIN
                    THROW 50001,
                    'Migration stopped: one or more food items have no matching category.',
                    1;
                END;
            ");

            // 4. Make Category mandatory.
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            // 5. Remove the old foreign-key relationship.
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodCategories_CategoryId",
                table: "FoodItems");

            migrationBuilder.DropIndex(
                name: "IX_FoodItems_CategoryId",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FoodItems");

            // 6. Remove the separate category table.
            migrationBuilder.DropTable(
                name: "FoodCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the category table.
            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    Description = table.Column<string>(
                        type: "nvarchar(250)",
                        maxLength: 250,
                        nullable: true),

                    IsActive = table.Column<bool>(
                        type: "bit",
                        nullable: false),

                    Name = table.Column<string>(
                        type: "nvarchar(50)",
                        maxLength: 50,
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategories_Name",
                table: "FoodCategories",
                column: "Name",
                unique: true);

            // Recreate CategoryId temporarily.
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "FoodItems",
                type: "int",
                nullable: true);

            // Recreate category records from existing food items.
            migrationBuilder.Sql(@"
                INSERT INTO FoodCategories (Name, IsActive)
                SELECT DISTINCT Category, 1
                FROM FoodItems
                WHERE Category IS NOT NULL;
            ");

            // Restore the foreign-key values.
            migrationBuilder.Sql(@"
                UPDATE FoodItems
                SET CategoryId = FoodCategories.Id
                FROM FoodItems
                INNER JOIN FoodCategories
                    ON FoodItems.Category = FoodCategories.Name;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "FoodItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_CategoryId",
                table: "FoodItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodCategories_CategoryId",
                table: "FoodItems",
                column: "CategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "FoodItems");
        }
    }
}