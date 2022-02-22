using Microsoft.EntityFrameworkCore.Migrations;

namespace PPMRm.Migrations
{
    public partial class Updated_PeriodReport_Navigations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPeriodReports_AppCSUpdates_CommoditySecurityUpdatesId",
                table: "AppPeriodReports");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId",
                table: "AppProductShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId1",
                table: "AppProductShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProductStocks_AppPeriodReports_PeriodReportId1",
                table: "AppProductStocks");

            migrationBuilder.DropTable(
                name: "AppCSUpdates");

            migrationBuilder.DropIndex(
                name: "IX_AppProductStocks_PeriodReportId1",
                table: "AppProductStocks");

            migrationBuilder.DropIndex(
                name: "IX_AppProductShipments_PeriodReportId1",
                table: "AppProductShipments");

            migrationBuilder.DropIndex(
                name: "IX_AppPeriodReports_CommoditySecurityUpdatesId",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "PeriodReportId1",
                table: "AppProductStocks");

            migrationBuilder.DropColumn(
                name: "PeriodReportId1",
                table: "AppProductShipments");

            migrationBuilder.RenameColumn(
                name: "CommoditySecurityUpdatesId",
                table: "AppPeriodReports",
                newName: "CommoditySecurityUpdates_WarehousingAndDistribution");

            migrationBuilder.AlterColumn<string>(
                name: "PeriodReportId",
                table: "AppProductShipments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_ForecastingAndSupplyPlanning",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_GovernanceAndFinancing",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_HumanResourcesCapacityDevelopmentAndT~",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_LogisticsManagementInformationSystem",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_ProcurementProductInformationAndRegis~",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_ProductStockLevelsInformation",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommoditySecurityUpdates_SupplyChainCommitteePolicyAndDonorCoo~",
                table: "AppPeriodReports",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId",
                table: "AppProductShipments",
                column: "PeriodReportId",
                principalTable: "AppPeriodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId",
                table: "AppProductShipments");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_ForecastingAndSupplyPlanning",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_GovernanceAndFinancing",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_HumanResourcesCapacityDevelopmentAndT~",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_LogisticsManagementInformationSystem",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_ProcurementProductInformationAndRegis~",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_ProductStockLevelsInformation",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CommoditySecurityUpdates_SupplyChainCommitteePolicyAndDonorCoo~",
                table: "AppPeriodReports");

            migrationBuilder.RenameColumn(
                name: "CommoditySecurityUpdates_WarehousingAndDistribution",
                table: "AppPeriodReports",
                newName: "CommoditySecurityUpdatesId");

            migrationBuilder.AddColumn<string>(
                name: "PeriodReportId1",
                table: "AppProductStocks",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PeriodReportId",
                table: "AppProductShipments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PeriodReportId1",
                table: "AppProductShipments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppCSUpdates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ForecastingAndSupplyPlanning = table.Column<string>(type: "text", nullable: true),
                    GovernanceAndFinancing = table.Column<string>(type: "text", nullable: true),
                    HumanResourcesCapacityDevelopmentAndTraining = table.Column<string>(type: "text", nullable: true),
                    LogisticsManagementInformationSystem = table.Column<string>(type: "text", nullable: true),
                    ProcurementProductInformationAndRegistration = table.Column<string>(type: "text", nullable: true),
                    ProductStockLevelsInformation = table.Column<string>(type: "text", nullable: true),
                    SupplyChainCommitteePolicyAndDonorCoordination = table.Column<string>(type: "text", nullable: true),
                    WarehousingAndDistribution = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_AppProductStocks_PeriodReportId1",
                table: "AppProductStocks",
                column: "PeriodReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductShipments_PeriodReportId1",
                table: "AppProductShipments",
                column: "PeriodReportId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppPeriodReports_CommoditySecurityUpdatesId",
                table: "AppPeriodReports",
                column: "CommoditySecurityUpdatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPeriodReports_AppCSUpdates_CommoditySecurityUpdatesId",
                table: "AppPeriodReports",
                column: "CommoditySecurityUpdatesId",
                principalTable: "AppCSUpdates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId",
                table: "AppProductShipments",
                column: "PeriodReportId",
                principalTable: "AppPeriodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductShipments_AppPeriodReports_PeriodReportId1",
                table: "AppProductShipments",
                column: "PeriodReportId1",
                principalTable: "AppPeriodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProductStocks_AppPeriodReports_PeriodReportId1",
                table: "AppProductStocks",
                column: "PeriodReportId1",
                principalTable: "AppPeriodReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
