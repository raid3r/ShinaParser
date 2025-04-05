using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShinaParser.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductsForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Countries_CountryId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Products",
                newName: "country_model_id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CountryId",
                table: "Products",
                newName: "IX_Products_country_model_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Countries_country_model_id",
                table: "Products",
                column: "country_model_id",
                principalTable: "Countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Countries_country_model_id",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "country_model_id",
                table: "Products",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_country_model_id",
                table: "Products",
                newName: "IX_Products_CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Countries_CountryId",
                table: "Products",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
