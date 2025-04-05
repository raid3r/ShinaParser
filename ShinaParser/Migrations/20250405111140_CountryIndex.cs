using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShinaParser.Migrations
{
    /// <inheritdoc />
    public partial class CountryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Countries_country_model_id",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_countries",
                table: "countries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_Title",
                table: "countries",
                column: "Title",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_countries_country_model_id",
                table: "Products",
                column: "country_model_id",
                principalTable: "countries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_countries_country_model_id",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_countries",
                table: "countries");

            migrationBuilder.DropIndex(
                name: "IX_countries_Title",
                table: "countries");

            migrationBuilder.RenameTable(
                name: "countries",
                newName: "Countries");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Countries_country_model_id",
                table: "Products",
                column: "country_model_id",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
