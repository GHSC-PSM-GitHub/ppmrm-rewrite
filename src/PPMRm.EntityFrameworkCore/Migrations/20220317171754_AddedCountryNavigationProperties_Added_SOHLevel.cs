using Microsoft.EntityFrameworkCore.Migrations;

namespace PPMRm.Migrations
{
    public partial class AddedCountryNavigationProperties_Added_SOHLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherSourceOfConsumption",
                table: "AppProductStocks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SOHLevels",
                table: "AppProductStocks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceOfConsumption",
                table: "AppProductStocks",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "DefaultProgramId",
                table: "AppCountries",
                type: "integer",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxStock",
                table: "AppCountries",
                type: "numeric",
                nullable: false,
                defaultValue: 12m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinStock",
                table: "AppCountries",
                type: "numeric",
                nullable: false,
                defaultValue: 6m);

            migrationBuilder.CreateTable(
                name: "AppCountryProducs",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCountryProducs", x => new { x.CountryId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_AppCountryProducs_AppCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "AppCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCountryProducs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppCountryPrograms",
                columns: table => new
                {
                    CountryId = table.Column<string>(type: "text", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCountryPrograms", x => new { x.CountryId, x.ProgramId });
                    table.ForeignKey(
                        name: "FK_AppCountryPrograms_AppCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "AppCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCountryPrograms_AppPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "AppPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCountries_DefaultProgramId",
                table: "AppCountries",
                column: "DefaultProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryProducs_CountryId_ProductId",
                table: "AppCountryProducs",
                columns: new[] { "CountryId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryProducs_ProductId",
                table: "AppCountryProducs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryPrograms_CountryId_ProgramId",
                table: "AppCountryPrograms",
                columns: new[] { "CountryId", "ProgramId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCountryPrograms_ProgramId",
                table: "AppCountryPrograms",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCountries_AppPrograms_DefaultProgramId",
                table: "AppCountries",
                column: "DefaultProgramId",
                principalTable: "AppPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCountries_AppPrograms_DefaultProgramId",
                table: "AppCountries");

            migrationBuilder.DropTable(
                name: "AppCountryProducs");

            migrationBuilder.DropTable(
                name: "AppCountryPrograms");

            migrationBuilder.DropIndex(
                name: "IX_AppCountries_DefaultProgramId",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "OtherSourceOfConsumption",
                table: "AppProductStocks");

            migrationBuilder.DropColumn(
                name: "SOHLevels",
                table: "AppProductStocks");

            migrationBuilder.DropColumn(
                name: "SourceOfConsumption",
                table: "AppProductStocks");

            migrationBuilder.DropColumn(
                name: "DefaultProgramId",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "MaxStock",
                table: "AppCountries");

            migrationBuilder.DropColumn(
                name: "MinStock",
                table: "AppCountries");
        }
    }
}
