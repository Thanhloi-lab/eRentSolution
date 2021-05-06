using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class game_la_de : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "87b37ff4-b9a1-4333-9b70-cedcfe01d294");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "a9248874-2e12-4334-bb0e-94d691119d3e");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "f3d4bae2-8a80-444d-9690-4297e408ca82", new DateTime(2021, 5, 3, 13, 51, 37, 194, DateTimeKind.Utc).AddTicks(3073), "AQAAAAEAACcQAAAAENA5Uo7u+VLUIAhw1N60mOLRGpm58f+3zl5Dql362nweO8s9bM1rNGlQOJUsuU5/MQ==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 3, 13, 51, 37, 230, DateTimeKind.Utc).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 3, 13, 51, 37, 229, DateTimeKind.Utc).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 3, 13, 51, 37, 229, DateTimeKind.Utc).AddTicks(940));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "0d10cb66-b8e7-4b07-80e6-0a73da9560af");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "474d7f90-aad3-449f-a976-151d0a8edfdf");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "83d69519-2184-4544-bae2-4386d79d8436", new DateTime(2021, 4, 30, 3, 10, 2, 567, DateTimeKind.Utc).AddTicks(3443), "AQAAAAEAACcQAAAAEDNM4qI6SJsubURcTAVqFy7CsNtqZVQq2lj7pU7995mJv6OHo1iA/hc4tzHbb2NVxg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 595, DateTimeKind.Utc).AddTicks(5650));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 594, DateTimeKind.Utc).AddTicks(6278));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 30, 3, 10, 2, 593, DateTimeKind.Utc).AddTicks(8381));
        }
    }
}
