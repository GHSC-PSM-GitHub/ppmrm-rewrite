using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPMRm.Migrations
{
    public partial class Added_PeriodReport_Shipment_ProductStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppPeriodReports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: false),
                    PeriodId = table.Column<int>(type: "integer", nullable: false),
                    CommoditySecurityUpdatesId = table.Column<string>(type: "text", nullable: true),
                    ReportStatus = table.Column<int>(type: "integer", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPeriodReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPeriodReports_AppCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "AppCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPeriodReports_AppPeriods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "AppPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppCSUpdates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ForecastingAndSupplyPlanning = table.Column<string>(type: "text", nullable: true),
                    ProcurementProductInformationAndRegistration = table.Column<string>(type: "text", nullable: true),
                    WarehousingAndDistribution = table.Column<string>(type: "text", nullable: true),
                    LogisticsManagementInformationSystem = table.Column<string>(type: "text", nullable: true),
                    GovernanceAndFinancing = table.Column<string>(type: "text", nullable: true),
                    HumanResourcesCapacityDevelopmentAndTraining = table.Column<string>(type: "text", nullable: true),
                    SupplyChainCommitteePolicyAndDonorCoordination = table.Column<string>(type: "text", nullable: true),
                    ProductStockLevelsInformation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCSUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCSUpdates_AppPeriodReports_Id",
                        column: x => x.Id,
                        principalTable: "AppPeriodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProductShipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PeriodReportId = table.Column<string>(type: "text", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Supplier = table.Column<int>(type: "integer", nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ShipmentDateType = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    DataSource = table.Column<int>(type: "integer", nullable: false),
                    PeriodReportId1 = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId",
                        column: x => x.PeriodReportId,
                        principalTable: "AppPeriodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId1",
                        column: x => x.PeriodReportId1,
                        principalTable: "AppPeriodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppProductShipments_AppPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "AppPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductShipments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProductStocks",
                columns: table => new
                {
                    PeriodReportId = table.Column<string>(type: "text", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    SOH = table.Column<decimal>(type: "numeric", nullable: false),
                    DateOfSOH = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AMC = table.Column<decimal>(type: "numeric", nullable: true),
                    ActionRecommended = table.Column<string>(type: "text", nullable: true),
                    DateActionNeededBy = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PeriodReportId1 = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductStocks", x => new { x.PeriodReportId, x.ProgramId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_AppProductStocks_AppPeriodReports_PeriodReportId",
                        column: x => x.PeriodReportId,
                        principalTable: "AppPeriodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductStocks_AppPeriodReports_PeriodReportId1",
                        column: x => x.PeriodReportId1,
                        principalTable: "AppPeriodReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppProductStocks_AppPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "AppPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProductStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPeriodReports_CommoditySecurityUpdatesId",
                table: "AppPeriodReports",
                column: "CommoditySecurityUpdatesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPeriodReports_CountryId_PeriodId",
                table: "AppPeriodReports",
                columns: new[] { "CountryId", "PeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppPeriodReports_PeriodId",
                table: "AppPeriodReports",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductShipments_PeriodReportId",
                table: "AppProductShipments",
                column: "PeriodReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductShipments_PeriodReportId1",
                table: "AppProductShipments",
                column: "PeriodReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductShipments_ProductId",
                table: "AppProductShipments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductShipments_ProgramId",
                table: "AppProductShipments",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStocks_PeriodReportId1",
                table: "AppProductStocks",
                column: "PeriodReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStocks_ProductId",
                table: "AppProductStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStocks_ProgramId",
                table: "AppProductStocks",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPeriodReports_AppCSUpdates_CommoditySecurityUpdatesId",
                table: "AppPeriodReports",
                column: "CommoditySecurityUpdatesId",
                principalTable: "AppCSUpdates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCSUpdates_AppPeriodReports_Id",
                table: "AppCSUpdates");

            migrationBuilder.DropTable(
                name: "AppProductShipments");

            migrationBuilder.DropTable(
                name: "AppProductStocks");

            migrationBuilder.DropTable(
                name: "AppPeriodReports");

            migrationBuilder.DropTable(
                name: "AppCSUpdates");
        }
    }
}
