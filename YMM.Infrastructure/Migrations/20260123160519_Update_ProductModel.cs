using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YMM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_ProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId1",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId1",
                table: "Products",
                column: "BrandId1",
                unique: true,
                filter: "[BrandId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId1",
                table: "Products",
                column: "BrandId1",
                principalTable: "Brands",
                principalColumn: "Id");
        }
    }
}
