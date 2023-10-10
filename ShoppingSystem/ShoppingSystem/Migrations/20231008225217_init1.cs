using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingSystem.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_CategoriesID",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoriesID",
                table: "Products",
                column: "CategoriesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_CategoriesID",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoriesID",
                table: "Products",
                column: "CategoriesID",
                unique: true);
        }
    }
}
