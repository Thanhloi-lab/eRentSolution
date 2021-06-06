using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eRentSolution.Data.Migrations
{
    public partial class fix_password : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "917545f1-cabe-46db-b69f-7a0afde3344c");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "6a1fba21-bc06-4dea-855c-1241df405a12");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "bf839714-0241-4e56-8de4-1efeee51e969", new DateTime(2021, 6, 6, 14, 49, 50, 610, DateTimeKind.Utc).AddTicks(9778), "AQAAAAEAACcQAAAAEP8+JIJXcLXjdRfpf7Rgr+fCYuFejosBk2RMHZhfhQV5X5xJHE+KYjdBTwem9CR7Ig==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 6, 6, 14, 49, 50, 637, DateTimeKind.Utc).AddTicks(5958));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 6, 14, 49, 50, 636, DateTimeKind.Utc).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 6, 14, 49, 50, 635, DateTimeKind.Utc).AddTicks(7904));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983de"),
                column: "ConcurrencyStamp",
                value: "8235eb62-82a7-4afc-bf77-0d65abd06048");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("e4df483b-524d-467b-b6f4-2ee002742987"),
                column: "ConcurrencyStamp",
                value: "34e98e13-85b9-4d88-a64f-d507b6db24b3");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00dd"),
                columns: new[] { "ConcurrencyStamp", "DateChangePassword", "PasswordHash" },
                values: new object[] { "a3f9a981-1fad-4856-aada-f1813ef47b8f", new DateTime(2021, 5, 7, 10, 43, 55, 652, DateTimeKind.Utc).AddTicks(3871), "AQAAAAEAACcQAAAAEAE8n0OAMDav9Bt6mv6PAmQZmcyrZkUjlapnOLh25pTjOlwfnuczeff7qHikO+qiSg==" });

            migrationBuilder.UpdateData(
                table: "Censors",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 680, DateTimeKind.Utc).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "ProductDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 679, DateTimeKind.Utc).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 5, 7, 10, 43, 55, 678, DateTimeKind.Utc).AddTicks(9921));
        }
    }
}
