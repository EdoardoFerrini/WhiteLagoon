using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlPropToVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 55, 8, 157, DateTimeKind.Local).AddTicks(1983), "https://placeholder.co/600x400" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 55, 8, 157, DateTimeKind.Local).AddTicks(2064), "https://placeholder.co/600x400" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 46, 34, 125, DateTimeKind.Local).AddTicks(7155), "" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedTime", "ImageUrl" },
                values: new object[] { new DateTime(2024, 2, 14, 15, 46, 34, 125, DateTimeKind.Local).AddTicks(7210), "" });
        }
    }
}
