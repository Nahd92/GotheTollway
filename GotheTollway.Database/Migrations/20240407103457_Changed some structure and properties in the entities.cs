using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotheTollway.Database.Migrations
{
    /// <inheritdoc />
    public partial class Changedsomestructureandpropertiesintheentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExemptedVehicleTypes",
                table: "TollFee");

            migrationBuilder.DropColumn(
                name: "ExemptionEndDate",
                table: "TollExemptions");

            migrationBuilder.DropColumn(
                name: "ExemptionStartDate",
                table: "TollExemptions");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Date",
                table: "TollPassages",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "TollPassages",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "TollFee",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "TollFee",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TollFee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TollExemptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExemptedVehicleTypes",
                table: "TollExemptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExemptionEndTime",
                table: "TollExemptions",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExemptionStartTime",
                table: "TollExemptions",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "TollPassages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TollFee");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TollExemptions");

            migrationBuilder.DropColumn(
                name: "ExemptedVehicleTypes",
                table: "TollExemptions");

            migrationBuilder.DropColumn(
                name: "ExemptionEndTime",
                table: "TollExemptions");

            migrationBuilder.DropColumn(
                name: "ExemptionStartTime",
                table: "TollExemptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TollPassages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "StartTime",
                table: "TollFee",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "EndTime",
                table: "TollFee",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<string>(
                name: "ExemptedVehicleTypes",
                table: "TollFee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExemptionEndDate",
                table: "TollExemptions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExemptionStartDate",
                table: "TollExemptions",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
