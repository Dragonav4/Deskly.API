using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hoteling.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Desks",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4080-9000-010203040506"));

            migrationBuilder.DeleteData(
                table: "Desks",
                keyColumn: "Id",
                keyValue: new Guid("d3e4f5a6-b7c8-4d0e-8f2e-3c4d5e6f7a8b"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Desks",
                columns: new[] { "Id", "DeskNumber", "Floor", "HasDualMonitor", "IsStandingDesk" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4080-9000-010203040506"), "B-005", 2, false, true },
                    { new Guid("d3e4f5a6-b7c8-4d0e-8f2e-3c4d5e6f7a8b"), "A-001", 1, true, false }
                });
        }
    }
}
