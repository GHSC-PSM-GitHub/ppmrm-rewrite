using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PPMRm.Migrations
{
    public partial class ModifiedPeriodReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppPeriodReports");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppPeriodReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppPeriodReports",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppPeriodReports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "AppPeriodReports",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppPeriodReports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppPeriodReports",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppPeriodReports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppPeriodReports",
                type: "uuid",
                nullable: true);
        }
    }
}
