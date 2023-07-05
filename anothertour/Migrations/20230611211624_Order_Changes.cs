using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace anothertour.Migrations
{
    /// <inheritdoc />
    public partial class Order_Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TourDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TouristsCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TourDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TouristsCount",
                table: "Orders");
        }
    }
}
