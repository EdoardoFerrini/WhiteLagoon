using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "CreatedTime", "Description", "ImageUrl", "Name", "Occupancy", "Price", "SquareFoot", "UpdatedTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 22, 18, 1, 7, 429, DateTimeKind.Local).AddTicks(8539), "This is a villa placed near the sea.", "", "Beach Villa", 5, 1000000.0, 1000, null },
                    { 2, new DateTime(2024, 1, 22, 18, 1, 7, 429, DateTimeKind.Local).AddTicks(8588), "This is a villa close to Colosseo.", "", "City Villa", 5, 1500000.0, 1200, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
