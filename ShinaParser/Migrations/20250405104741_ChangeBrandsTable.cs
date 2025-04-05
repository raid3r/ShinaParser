using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShinaParser.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBrandsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "product_brands");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "product_brands",
                newName: "brand_name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "product_brands",
                newName: "brand_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_brands",
                table: "product_brands",
                column: "brand_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_product_brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "product_brands",
                principalColumn: "brand_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_product_brands_BrandId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_brands",
                table: "product_brands");

            migrationBuilder.RenameTable(
                name: "product_brands",
                newName: "Brands");

            migrationBuilder.RenameColumn(
                name: "brand_name",
                table: "Brands",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "brand_id",
                table: "Brands",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
